using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using MediatR;

namespace ConsiliumTempus.Application.User.Events;

public sealed class UserDeletedHandler(IWorkspaceRepository workspaceRepository)
    : INotificationHandler<UserDeleted>
{
    public async Task Handle(UserDeleted notification, CancellationToken cancellationToken)
    {
        var workspaces =
            await workspaceRepository.GetListForUserWithMemberships(notification.User, cancellationToken);

        foreach (var workspace in workspaces)
        {
            if (workspace.Memberships.Count == 1)
            {
                workspaceRepository.Remove(workspace);
            }
            else if (workspace.Owner == notification.User) 
            {
                // transfer ownership, and promote role if necessary
                var newOwnerAdmin = workspace.Memberships
                    .FirstOrDefault(m => m.User != notification.User &&
                                         m.WorkspaceRole.Equals(WorkspaceRole.Admin));
                var newOwner = newOwnerAdmin ?? workspace.Memberships.First(m => m.User != notification.User);
                if (newOwnerAdmin is null) newOwner.UpdateWorkspaceRole(WorkspaceRole.Admin);
                workspace.TransferOwnership(newOwner.User);
                workspace.UpdateIsUserWorkspace(IsUserWorkspace.Create(false));
            }
        }
    }
}