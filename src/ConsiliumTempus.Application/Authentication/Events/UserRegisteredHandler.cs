using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Events;

public class UserRegisteredHandler : INotificationHandler<UserRegistered>
{
    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var workspace = WorkspaceAggregate.Create(
            "My Workspace",
            "This is your workspace, where you can create anything you desire. " +
            "Take a quick look on our features");

        notification.User.AddWorkspace(workspace);

        await Task.CompletedTask;
    }
}