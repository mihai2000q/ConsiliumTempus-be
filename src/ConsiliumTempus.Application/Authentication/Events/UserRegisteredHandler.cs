using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;
using MediatR;
using Constants = ConsiliumTempus.Domain.Common.Constants.Constants;

namespace ConsiliumTempus.Application.Authentication.Events;

public class UserRegisteredHandler(IWorkspaceRoleRepository workspaceRoleRepository) : INotificationHandler<UserRegistered>
{
    public Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var workspace = WorkspaceAggregate.Create(
            Constants.Workspace.Name,
            Constants.Workspace.Description);

        var role = WorkspaceRole.Admin;
        workspaceRoleRepository.Attach(role);
        
        var membership = Membership.Create(notification.User, workspace, role);
        notification.User.AddWorkspaceMembership(membership);
        
        return Task.CompletedTask;
    }
}