using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Events;

public class UserRegisteredHandler(IWorkspaceRoleRepository workspaceRoleRepository) : INotificationHandler<UserRegistered>
{
    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var workspace = WorkspaceAggregate.Create(
            "My Workspace",
            "This is your workspace, where you can create anything you desire. " +
            "Take a quick look on our features");

        var role = WorkspaceRole.Admin;
        workspaceRoleRepository.Attach(role);
        
        var membership = Membership.Create(notification.User, workspace, role);
        notification.User.AddWorkspaceMembership(membership);
        
        await Task.CompletedTask;
    }
}