using System.Diagnostics.CodeAnalysis;
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
        SelectedOption? option,
        SingleSelectCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate task) : base(id, task)
    {
        Option = option;
        Setup = setup;
    }

    public SelectedOption? Option { get; init; }
    public SingleSelectCustomFieldSetupAggregate Setup { get; init; } = null!;

    public static SingleSelectCustomField Create(
        SelectedOption? option,
        SingleSelectCustomFieldSetupAggregate setup,
        ProjectTaskAggregate task)
    {
        return new SingleSelectCustomField(
            option,
            setup,
            CustomFieldId.CreateUnique(),
            task);
    }
}