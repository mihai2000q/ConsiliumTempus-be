using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Update;

public sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateUserCommand, ErrorOr<UpdateUserResult>>
{
    public async Task<ErrorOr<UpdateUserResult>> Handle(UpdateUserCommand command, 
        CancellationToken cancellationToken)
    {
        var userId = UserId.Create(command.Id);
        var user = await userRepository.Get(userId, cancellationToken);

        if (user is null) return Errors.User.NotFound;

        user.Update(
            Name.Create(
                command.FirstName.Capitalize(),
                command.LastName.Capitalize()),
            command.Role,
            command.DateOfBirth);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateUserResult(user);
    }
}