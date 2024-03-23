using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.Delete;

public sealed class DeleteUserCommandHandler(
    IUserRepository userRepository, 
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<DeleteUserCommand, ErrorOr<DeleteUserResult>>
{
    public async Task<ErrorOr<DeleteUserResult>> Handle(DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);
        
        userRepository.Remove(user);
        
        return new DeleteUserResult();
    }
}