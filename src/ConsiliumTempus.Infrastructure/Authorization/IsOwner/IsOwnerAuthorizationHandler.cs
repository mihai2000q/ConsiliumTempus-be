using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Http;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Authorization.IsOwner;

public sealed class IsOwnerAuthorizationHandler(IServiceScopeFactory serviceScopeFactory) 
    : AuthorizationHandler<IsOwnerRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        IsOwnerRequirement requirement)
    {
        var claims = context.User.Claims.ToArray();
        var jwtUserId = claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        
        using var scope = serviceScopeFactory.CreateScope();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var userProvider = scope.ServiceProvider.GetRequiredService<IUserProvider>();
        var user = await userProvider.Get(UserId.Create(jwtUserId));
        
        if (user is null) return;
        
        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;
        
        var res = await GetUserId(request, userProvider);
        if (res is null) return;
        if (res.NotFound)
        {
            context.Succeed(requirement); // let the system return the not found error
        }
        else
        {
            if (res.User! == user) context.Succeed(requirement);
        }
    }

    private static async Task<UserIdResponse?> GetUserId(
        HttpRequest request,
        IUserProvider userProvider)
    {
        var stringId = request.RouteValues["controller"] switch
        {
            ApiControllers.User => request.Method switch
            {
                HttpRequestType.GET => HttpRequestReader.GetStringIdFromRoute(request),
                HttpRequestType.PUT => await HttpRequestReader.GetStringIdFromBody(request),
                HttpRequestType.DELETE => HttpRequestReader.GetStringIdFromRoute(request),
                _ => null
            },
            _ => null
        };
        
        if (string.IsNullOrWhiteSpace(stringId)) return null;
        if (!Guid.TryParse(stringId, out var guidUserId)) return null;

        var userId = UserId.Create(guidUserId);
        var user = await userProvider.Get(userId);

        return user is null
            ? new UserIdResponse(user, true)
            : new UserIdResponse(user, false);
    }
    
    private class UserIdResponse(UserAggregate? user, bool notFound)
    {
        internal UserAggregate? User { get; } = user;
        internal bool NotFound { get; } = notFound;
    }
}