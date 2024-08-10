using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;

public sealed class CustomFieldSetupId : AggregateRootId<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private CustomFieldSetupId()
    {
    }

    private CustomFieldSetupId(Guid value)
    {
        Value = value;
    }

    public override Guid Value { get; protected set; }

    public static CustomFieldSetupId CreateUnique()
    {
        return new CustomFieldSetupId(Guid.NewGuid());
    }

    public static CustomFieldSetupId Create(Guid value)
    {
        return new CustomFieldSetupId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}