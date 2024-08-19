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
        CustomFieldId id,
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        Option = option;
        Setup = setup;
    }

    public SingleSelectOption? Option { get; private set; }
    public SingleSelectCustomFieldSetupAggregate Setup { get; init; } = null!;

    public static SingleSelectCustomField Create(
        SingleSelectOption? option,
        SingleSelectCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new SingleSelectCustomField(
            option,
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }

    public void Update(SingleSelectOption? option)
    {
        Option = option;
    }
}