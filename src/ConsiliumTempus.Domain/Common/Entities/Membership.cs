using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Events;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class Membership : Entity<(UserId, WorkspaceId)>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Membership()
    {
    }

    private Membership(
        UserAggregate user,
        WorkspaceAggregate workspace,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        WorkspaceRole workspaceRole) : base((user.Id, workspace.Id))
    {
        User = user;
        Workspace = workspace;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        WorkspaceRole = workspaceRole;
    }
    
    public UserAggregate User { get; init; } = null!;
    public WorkspaceAggregate Workspace { get; init; } = null!;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public WorkspaceRole WorkspaceRole { get; private set; } = null!;

    public static Membership Create(
        UserAggregate user,
        WorkspaceAggregate workspace,
        WorkspaceRole workspaceRole)
    {
        var membership = new Membership(
            user, 
            workspace,
            DateTime.UtcNow, 
            DateTime.UtcNow, 
            workspaceRole);

        membership.AddDomainEvent(new MembershipCreated(membership));
        
        return membership;
    }

    public void UpdateWorkspaceRole(WorkspaceRole role)
    {
        WorkspaceRole = role;
        UpdatedDateTime = DateTime.UtcNow;
    }
}