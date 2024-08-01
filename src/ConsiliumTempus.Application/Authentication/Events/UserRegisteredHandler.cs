using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using MediatR;
using Constants = ConsiliumTempus.Domain.Common.Constants.Constants;

namespace ConsiliumTempus.Application.Authentication.Events;

public sealed class UserRegisteredHandler(IWorkspaceRepository workspaceRepository) 
    : INotificationHandler<UserRegistered>
{
    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        var workspace = WorkspaceAggregate.Create(
            Name.Create(Constants.Workspace.Name),
            Description.Create(Constants.Workspace.Description),
            notification.User,
            IsPersonal.Create(true));
        await workspaceRepository.Add(workspace, cancellationToken);
    }
}