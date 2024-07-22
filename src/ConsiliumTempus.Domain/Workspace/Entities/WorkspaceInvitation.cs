using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Workspace.Entities;

public sealed class WorkspaceInvitation : Entity<WorkspaceInvitationId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceInvitation()
    {
    }

    private WorkspaceInvitation(
        WorkspaceInvitationId id,
        DateTime createdDateTime,
        UserAggregate sender,
        UserAggregate collaborator,
        WorkspaceAggregate workspace) : base(id)
    {
        CreatedDateTime = createdDateTime;
        Collaborator = collaborator;
        Sender = sender;
        Workspace = workspace;
    }

    public DateTime CreatedDateTime { get; init; }
    public UserAggregate Sender { get; init; } = default!;
    public UserAggregate Collaborator { get; init; } = default!;
    public WorkspaceAggregate Workspace { get; init; } = default!;

    public static WorkspaceInvitation Create(
        UserAggregate sender,
        UserAggregate collaborator,
        WorkspaceAggregate workspace)
    {
        return new WorkspaceInvitation(
            WorkspaceInvitationId.CreateUnique(),
            DateTime.UtcNow,
            sender,
            collaborator,
            workspace);
    }
}