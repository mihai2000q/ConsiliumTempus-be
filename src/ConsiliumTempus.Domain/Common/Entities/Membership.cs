using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class Membership : Entity<(UserId UserId, WorkspaceId WorkspaceId)>, ITimestamps
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
        int workspaceRoleId) : base((user.Id, workspace.Id))
    {
        User = user;
        Workspace = workspace;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        _workspaceRoleId = workspaceRoleId;
    }

    private int _workspaceRoleId;

    public override (UserId UserId, WorkspaceId WorkspaceId) Id => new(User.Id, Workspace.Id);
    public UserAggregate User { get; init; } = null!;
    public WorkspaceAggregate Workspace { get; init; } = null!;
    public WorkspaceRole WorkspaceRole => WorkspaceRole.GetValues().Single(wr => wr.Id == _workspaceRoleId);
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }

    public static Membership Create(
        UserAggregate user,
        WorkspaceAggregate workspace,
        WorkspaceRole workspaceRole)
    {
        return new Membership(
            user,
            workspace,
            DateTime.UtcNow,
            DateTime.UtcNow,
            workspaceRole.Id);
    }

    public void Update(WorkspaceRole role)
    {
        _workspaceRoleId = role.Id;
        UpdatedDateTime = DateTime.UtcNow;
    }
}