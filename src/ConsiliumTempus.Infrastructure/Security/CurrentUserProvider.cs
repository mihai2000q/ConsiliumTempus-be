using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security;

internal sealed class CurrentUserProvider(
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
        return await GetCurrentUser(cancellationToken) ??
               throw new InvalidOperationException(
                   "The current user could not be found, although the permission check has passed.");
    }
}