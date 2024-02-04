using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IUserRepository userRepository,
    IScrambler scrambler,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginQuery, ErrorOr<LoginResult>>
{
    public async Task<ErrorOr<LoginResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmail(query.Email);
        if (user is null) return Errors.Authentication.InvalidCredentials;

        var isPasswordEqual = scrambler.VerifyPassword(query.Password, user.Password);
        if (!isPasswordEqual) return Errors.Authentication.InvalidCredentials;

        var token = jwtTokenGenerator.GenerateToken(user);

        return new LoginResult(token);
    }
}