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

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IScrambler scrambler,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.GetUserByEmail(command.Email) is not null) return Errors.User.DuplicateEmail;

        var password = scrambler.HashPassword(command.Password);

        var user = UserAggregate.Register(
            Credentials.Create(
                command.Email.ToLower(),
                password), 
            Name.Create(
                command.FirstName.Capitalize(),
                command.LastName.Capitalize()));
        await userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var token = jwtTokenGenerator.GenerateToken(user);

        return new RegisterResult(token);
    }
}