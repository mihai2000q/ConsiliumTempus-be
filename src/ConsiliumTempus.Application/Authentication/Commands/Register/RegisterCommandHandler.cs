using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Register;

public sealed class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IScrambler scrambler,
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.ToLower();
        if (await userRepository.GetUserByEmail(email, cancellationToken) is not null)
            return Errors.User.DuplicateEmail;

        var password = scrambler.HashPassword(command.Password);

        var user = UserAggregate.Register(
            Credentials.Create(email, password),
            FirstName.Create(command.FirstName.Capitalize()),
            LastName.Create(command.LastName.Capitalize()),
            command.Role.IfNotNull(Role.Create(command.Role!)),
            command.DateOfBirth);
        await userRepository.Add(user, cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user);
        var jwtId = jwtTokenGenerator.GetJwtIdFromToken(token);

        var refreshToken = RefreshToken.Create(jwtId, user);
        await refreshTokenRepository.Add(refreshToken, cancellationToken);

        return new RegisterResult(token, refreshToken.Value);
    }
}