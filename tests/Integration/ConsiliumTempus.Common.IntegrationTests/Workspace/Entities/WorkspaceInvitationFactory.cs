using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.Workspace.Entities;

public static class WorkspaceInvitationFactory
{
    public static WorkspaceInvitation Create(
        UserAggregate sender,
        UserAggregate collaborator,
        WorkspaceAggregate workspace,
        DateTime? createdDateTime = null)
    {
        return EntityBuilder<WorkspaceInvitation>.Empty()
            .WithProperty(nameof(WorkspaceInvitation.Id), WorkspaceInvitationId.CreateUnique())
            .WithProperty(nameof(WorkspaceInvitation.Sender), sender)
            .WithProperty(nameof(WorkspaceInvitation.Collaborator), collaborator)
            .WithProperty(nameof(WorkspaceInvitation.Workspace), workspace)
            .WithProperty(nameof(WorkspaceInvitation.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .Build();
    }
}