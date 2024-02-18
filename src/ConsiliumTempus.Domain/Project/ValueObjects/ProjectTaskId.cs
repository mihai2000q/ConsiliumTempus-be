using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Project.ValueObjects;

public sealed class ProjectTaskId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectTaskId()
    {
    }

    private ProjectTaskId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

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