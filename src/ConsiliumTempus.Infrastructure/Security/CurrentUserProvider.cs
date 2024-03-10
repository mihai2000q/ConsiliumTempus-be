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
    public async Task<UserAggregate> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var jwtUserId = httpContextAccessor.HttpContext!.User.Claims
            .Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        var userId = UserId.Create(jwtUserId);
        var user = await userRepository.Get(userId, cancellationToken);
        return user!;
    }
}