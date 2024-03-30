using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
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

        if (user is null) return Errors.User.NotFound;
        
        userRepository.Remove(user);
        
        return new DeleteUserResult();
    }
}