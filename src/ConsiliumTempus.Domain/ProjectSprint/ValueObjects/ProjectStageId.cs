using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

public sealed class ProjectStageId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectStageId()
    {
    }

    private ProjectStageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ProjectStageId CreateUnique()
    {
        return new ProjectStageId(Guid.NewGuid());
    }

    public static ProjectStageId Create(Guid value)
    {
        return new ProjectStageId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}