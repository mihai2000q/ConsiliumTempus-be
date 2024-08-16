using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class NumberCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private NumberCustomField()
    {
    }

    private NumberCustomField(
        DecimalNumber? number,
        NumberCustomFieldSetupAggregate setup,
        CustomFieldId id) : base(id)
    {
        Number = number;
        Setup = setup;
    }

    public DecimalNumber? Number { get; init; }
    public NumberCustomFieldSetupAggregate Setup { get; init; } = null!;

    public static NumberCustomField Create(
        DecimalNumber? decimalNumber,
        NumberCustomFieldSetupAggregate setup)
    {
        return new NumberCustomField(
            decimalNumber,
            setup,
            CustomFieldId.CreateUnique());
    }
}