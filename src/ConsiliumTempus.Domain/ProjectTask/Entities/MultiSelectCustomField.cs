using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class MultiSelectCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private MultiSelectCustomField()
    {
    }

    private MultiSelectCustomField(
        MultiSelectCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        Setup = setup;
    }

    private readonly List<MultiSelectOption> _options = [];

    public IReadOnlyList<MultiSelectOption> Options => _options
        .OrderBy(o => o.CustomOrderPosition)
        .ToList()
        .AsReadOnly();

    public override MultiSelectCustomFieldSetupAggregate Setup { get; } = null!;

    public static MultiSelectCustomField Create(
        MultiSelectCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new MultiSelectCustomField(
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }

    public void AddOption(MultiSelectOption option)
    {
        _options.Add(option);
    }

    public void RemoveOption(MultiSelectOption option)
    {
        _options.Remove(option);
    }
}