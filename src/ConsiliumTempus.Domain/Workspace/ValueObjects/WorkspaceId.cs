using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Workspace.ValueObjects;

public sealed class WorkspaceId : AggregateRootId<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceId()
    {
    }

    private WorkspaceId(Guid value)
    {
        Value = value;
    }

    public override Guid Value { get; protected set; }

    public static WorkspaceId CreateUnique()
    {
        return new WorkspaceId(Guid.NewGuid());
    }

    public static WorkspaceId Create(Guid value)
    {
        return new WorkspaceId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}