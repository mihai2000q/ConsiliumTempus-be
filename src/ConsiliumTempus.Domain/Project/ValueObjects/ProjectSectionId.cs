using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Project.ValueObjects;

public sealed class ProjectSectionId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectSectionId()
    {
    }

    private ProjectSectionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static ProjectSectionId CreateUnique()
    {
        return new ProjectSectionId(Guid.NewGuid());
    }

    public static ProjectSectionId Create(Guid value)
    {
        return new ProjectSectionId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}