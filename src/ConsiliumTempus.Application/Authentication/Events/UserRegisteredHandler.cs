using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using MediatR;
using Constants = ConsiliumTempus.Domain.Common.Constants.Constants;

namespace ConsiliumTempus.Application.Authentication.Events;

public sealed class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
    public Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var workspace = WorkspaceAggregate.Create(
            Name.Create(Constants.Workspace.Name),
            notification.User,
            IsPersonal.Create(true));
        workspace.Update(workspace.Name, Description.Create(Constants.Workspace.Description));

        var membership = Membership.Create(notification.User, workspace, WorkspaceRole.Admin);
        notification.User.AddWorkspaceMembership(membership);

        return Task.CompletedTask;
    }
}