using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Delete;

public sealed class DeleteUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserCommand, ErrorOr<DeleteUserResult>>
{
    public async Task<ErrorOr<DeleteUserResult>> Handle(DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var userId = UserId.Create(command.Id);
        var user = await userRepository.Get(userId, cancellationToken);

        if (user is null) return Errors.User.NotFound;

        userRepository.Remove(user);
        
        return new DeleteUserResult();
    }
}