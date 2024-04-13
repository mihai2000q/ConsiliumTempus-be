using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Project.ValueObjects;

public sealed class ProjectId : AggregateRootId<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectId()
    {
    }

    private ProjectId(Guid value)
    {
        Value = value;
    }

    public override Guid Value { get; protected set; }

    public static ProjectId CreateUnique()
    {
        return new ProjectId(Guid.NewGuid());
    }

    public static ProjectId Create(Guid value)
    {
        return new ProjectId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}