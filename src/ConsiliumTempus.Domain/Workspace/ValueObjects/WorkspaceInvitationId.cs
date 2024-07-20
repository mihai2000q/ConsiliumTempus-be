using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Workspace.ValueObjects;

public sealed class WorkspaceInvitationId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceInvitationId()
    {
    }

    private WorkspaceInvitationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static WorkspaceInvitationId CreateUnique()
    {
        return new WorkspaceInvitationId(Guid.NewGuid());
    }

    public static WorkspaceInvitationId Create(Guid value)
    {
        return new WorkspaceInvitationId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}