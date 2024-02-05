using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;

namespace ConsiliumTempus.Application.Common.Security;

public class Security(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
{
    public async Task<ErrorOr<UserAggregate>> GetUserFromToken(string plainToken)
    {
        var jwtUserId = jwtTokenGenerator.GetUserIdFromToken(plainToken);
        
        if (jwtUserId.IsError) return jwtUserId.Errors;

        var userId = UserId.Create(new Guid(jwtUserId.Value));
        var user = await userRepository.Get(userId);

        if (user is null) return Errors.Authentication.InvalidToken;

        return user;
    }
}