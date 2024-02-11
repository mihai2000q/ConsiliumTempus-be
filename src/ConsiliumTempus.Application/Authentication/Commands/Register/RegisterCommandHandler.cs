using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Authentication;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
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
    IUnitOfWork unitOfWork)
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
            Name.Create(
                command.FirstName.Capitalize(),
                command.LastName.Capitalize()));
        await userRepository.Add(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user);

        return new RegisterResult(token);
    }
}