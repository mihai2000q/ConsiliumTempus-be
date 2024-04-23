using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security;

public class CurrentUserProvider(
    IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository)
    : ICurrentUserProvider
{
    public async Task<UserAggregate?> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var subUserId = httpContextAccessor.HttpContext?.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (subUserId is null || !Guid.TryParse(subUserId, out var guidUserId)) return null;
        var userId = UserId.Create(guidUserId);
        var user = await userRepository.Get(userId, cancellationToken);
        return user;
    }
    
    public async Task<UserAggregate> GetCurrentUserAfterPermissionCheck(CancellationToken cancellationToken = default)
    {
        var subUserId = httpContextAccessor.HttpContext!.User.Claims
            .Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        var userId = UserId.Create(Guid.Parse(subUserId));
        var user = await userRepository.Get(userId, cancellationToken);
        return user!;
    }
}