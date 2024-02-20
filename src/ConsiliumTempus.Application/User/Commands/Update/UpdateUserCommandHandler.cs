using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Update;

public sealed class UpdateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserCommand, ErrorOr<UpdateUserResult>>
{
    public async Task<ErrorOr<UpdateUserResult>> Handle(UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var userId = UserId.Create(command.Id);
        var user = await userRepository.Get(userId, cancellationToken);

        if (user is null) return Errors.User.NotFound;

        user.Update(
            FirstName.Create(command.FirstName.Capitalize()),
            LastName.Create(command.LastName.Capitalize()),
            command.Role is null ? null : Role.Create(command.Role),
            command.DateOfBirth);

        return new UpdateUserResult(user);
    }
}