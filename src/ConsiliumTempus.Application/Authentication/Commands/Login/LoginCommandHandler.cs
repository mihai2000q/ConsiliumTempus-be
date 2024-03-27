using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IScrambler scrambler,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, ErrorOr<LoginResult>>
{
    public async Task<ErrorOr<LoginResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmail(command.Email.ToLower(), cancellationToken);
        if (user is null) return Errors.Authentication.InvalidCredentials;

        var isPasswordEqual = scrambler.VerifyPassword(command.Password, user.Credentials.Password);
        if (!isPasswordEqual) return Errors.Authentication.InvalidCredentials;

        var token = jwtTokenGenerator.GenerateToken(user);
        var jwtId = jwtTokenGenerator.GetJwtIdFromToken(token);
        var refreshToken = RefreshToken.Create(token, jwtId, user);
        await refreshTokenRepository.Add(refreshToken, cancellationToken);
        
        return new LoginResult(token, refreshToken.Value);
    }
}