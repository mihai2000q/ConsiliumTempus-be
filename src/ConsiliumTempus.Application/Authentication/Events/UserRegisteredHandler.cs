using ConsiliumTempus.Domain.UserAggregate.Events;
using ConsiliumTempus.Domain.WorkspaceAggregate;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Events;

public class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var workspace = Workspace.Create(
            "My Workspace",
            "This is your workspace, where you can create anything you desire. " +
            "Take a quick look on our features");

        notification.User.AddWorkspace(workspace);

        await Task.CompletedTask;
    }
}