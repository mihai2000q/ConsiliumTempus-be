using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class SingleSelectCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private SingleSelectCustomField()
    {
    }

    private SingleSelectCustomField(
        SingleSelectOption? option,
        SingleSelectCustomFieldSetupAggregate setup,
        CustomFieldId id) : base(id)
    {
        Option = option;
        Setup = setup;
    }

    public SingleSelectOption? Option { get; init; }
    public SingleSelectCustomFieldSetupAggregate Setup { get; init; } = null!;

    public static SingleSelectCustomField Create(
        SingleSelectOption? option,
        SingleSelectCustomFieldSetupAggregate setup)
    {
        return new SingleSelectCustomField(
            option,
            setup,
            CustomFieldId.CreateUnique());
    }
}