using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Project.ValueObjects;

public sealed class ProjectStatusId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectStatusId()
    {
    }

    private ProjectStatusId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ProjectStatusId CreateUnique()
    {
        return new ProjectStatusId(Guid.NewGuid());
    }

    public static ProjectStatusId Create(Guid value)
    {
        return new ProjectStatusId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}