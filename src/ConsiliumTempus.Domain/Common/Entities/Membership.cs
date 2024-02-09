using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Entities;

public class Membership : Entity<Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Membership()
    {
    }

    private Membership(
        Guid id,
        UserAggregate user,
        WorkspaceAggregate workspace,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        WorkspaceRole workspaceRole) : base(id)
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
    public DateTime UpdatedDateTime { get; init; }
    public WorkspaceRole WorkspaceRole { get; init; } = null!;

    public static Membership Create(
        UserAggregate user,
        WorkspaceAggregate workspace,
        WorkspaceRole workspaceRole)
    {
        return new Membership(
            Guid.NewGuid(),
            user, 
            workspace,
            DateTime.UtcNow, 
            DateTime.UtcNow, 
            workspaceRole);
    }
}