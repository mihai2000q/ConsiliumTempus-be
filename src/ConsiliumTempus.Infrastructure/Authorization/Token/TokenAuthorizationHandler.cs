using System.Security.Claims;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Authorization.Token;

public sealed class TokenAuthorizationHandler(IServiceScopeFactory serviceScopeFactory) 
    : AuthorizationHandler<TokenRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        TokenRequirement requirement)
    {
        var claims = context.User.Claims.ToArray();
        var jwtUserId = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(jwtUserId, out var userId)) return;

        using var scope = serviceScopeFactory.CreateScope();
        var userProvider = scope.ServiceProvider.GetRequiredService<IUserProvider>();
        var user = await userProvider.Get(UserId.Create(userId));
        
        if (user is null) return;
        
        if (AreClaimsValid(claims, user)) context.Succeed(requirement);
    }

    private static bool AreClaimsValid(Claim[] claims, UserAggregate user)
    {
        return GetClaim(claims, JwtRegisteredClaimNames.GivenName) == user.Name.First &&
               GetClaim(claims, JwtRegisteredClaimNames.FamilyName) == user.Name.Last &&
               GetClaim(claims, JwtRegisteredClaimNames.Email) == user.Credentials.Email &&
               GetClaim(claims, JwtRegisteredClaimNames.Jti)?.Length == 36;
    }

    private static string? GetClaim(IEnumerable<Claim> claims, string type)
    {
        return claims.FirstOrDefault(x => x.Type == type)?.Value;
    }
}