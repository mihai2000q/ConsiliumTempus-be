using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.ProjectTask.ValueObjects;

public sealed class ProjectTaskId : AggregateRootId<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectTaskId()
    {
    }

    private ProjectTaskId(Guid value)
    {
        Value = value;
    }

    public override Guid Value { get; protected set; }

    public static ProjectTaskId CreateUnique()
    {
        return new ProjectTaskId(Guid.NewGuid());
    }

    public static ProjectTaskId Create(Guid value)
    {
        return new ProjectTaskId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}