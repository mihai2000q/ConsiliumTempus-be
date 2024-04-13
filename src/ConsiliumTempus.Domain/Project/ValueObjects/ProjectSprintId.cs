using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Project.ValueObjects;

public sealed class ProjectSprintId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectSprintId()
    {
    }

    private ProjectSprintId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static ProjectSprintId CreateUnique()
    {
        return new ProjectSprintId(Guid.NewGuid());
    }

    public static ProjectSprintId Create(Guid value)
    {
        return new ProjectSprintId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}