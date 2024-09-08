using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.ProjectTask.ValueObjects;

public sealed class CustomFieldId : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private CustomFieldId()
    {
    }

    private CustomFieldId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CustomFieldId CreateUnique()
    {
        return new CustomFieldId(Guid.NewGuid());
    }

    public static CustomFieldId Create(Guid value)
    {
        return new CustomFieldId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}