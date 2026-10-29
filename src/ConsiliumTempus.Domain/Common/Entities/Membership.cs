using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class Membership : Entity<(UserId UserId, WorkspaceId WorkspaceId)>, IAudit
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Membership()
    {
    }

    private Membership(
        UserAggregate user,
        WorkspaceAggregate workspace,
        int workspaceRoleId) : base((user.Id, workspace.Id))
    {
        User = user;
        Workspace = workspace;
        _workspaceRoleId = workspaceRoleId;
    }

    private int _workspaceRoleId; // used to persist in the database
    private readonly WorkspaceRole? _workspaceRole; // used to retrieve from database

    public override (UserId UserId, WorkspaceId WorkspaceId) Id => new(User.Id, Workspace.Id);
    public UserAggregate User { get; init; } = null!;
    public WorkspaceAggregate Workspace { get; init; } = null!;
    public WorkspaceRole WorkspaceRole
    {
        get => _workspaceRole ?? WorkspaceRole.GetValues().Single(wr => wr.Id == _workspaceRoleId);
        init => _workspaceRole = value;
    }

    public Audit Audit { get; init; } = null!;

    public static Membership Create(
        UserAggregate user,
        WorkspaceAggregate workspace,
        WorkspaceRole workspaceRole)
    {
        return new Membership(
            user,
            workspace,
            workspaceRole.Id);
    }

    public void Update(WorkspaceRole role)
    {
        _workspaceRoleId = role.Id;
    }
}