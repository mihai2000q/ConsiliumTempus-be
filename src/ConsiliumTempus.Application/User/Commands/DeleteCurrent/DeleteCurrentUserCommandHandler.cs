using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User.Events;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.User.Commands.DeleteCurrent;

public sealed class DeleteCurrentUserCommandHandler(
    IUserRepository userRepository,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<DeleteCurrentUserCommand, ErrorOr<DeleteCurrentUserResult>>
{
    public async Task<ErrorOr<DeleteCurrentUserResult>> Handle(DeleteCurrentUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await currentUserProvider.GetCurrentUser(cancellationToken);

        if (user is null) return Errors.User.NotFound;

        userRepository.Remove(user);
        user.AddDomainEvent(new UserDeleted(user));

        return new DeleteCurrentUserResult();
    }
}