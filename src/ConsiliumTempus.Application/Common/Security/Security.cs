using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.Common.Security;

public sealed class Security(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository) : ISecurity
{
    public async Task<UserAggregate> GetUserFromToken(string plainToken, CancellationToken cancellationToken = default)
    {
        var jwtUserId = jwtTokenGenerator.GetUserIdFromToken(plainToken);
        var userId = UserId.Create(jwtUserId);
        var user = await userRepository.Get(userId, cancellationToken);
        return user!;
    }
}