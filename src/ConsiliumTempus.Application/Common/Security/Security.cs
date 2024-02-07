using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.Common.Security;

public class Security(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository) : ISecurity
{
    public async Task<UserAggregate> GetUserFromToken(string plainToken)
    {
        var jwtUserId = jwtTokenGenerator.GetUserIdFromToken(plainToken);
        var userId = UserId.Create(jwtUserId);
        var user = await userRepository.Get(userId);
        return user!;
    }
}