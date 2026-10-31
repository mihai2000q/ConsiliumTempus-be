using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Events;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class SingleSelectCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private SingleSelectCustomFieldSetupAggregate()
    {
    }

    private SingleSelectCustomFieldSetupAggregate(
        List<SingleSelectOption> options,
        Guid? defaultOptionId,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace) : base(id, name, description, workspace)
    {
        _options = options;
        DefaultOptionId = defaultOptionId;
    }

    private readonly List<SingleSelectOption> _options = [];
    
    public Guid? DefaultOptionId { get; private set; }
    public SingleSelectOption? DefaultOption => _options.SingleOrDefault(o => o.Id == DefaultOptionId);

    public IReadOnlyList<SingleSelectOption> Options => _options
        .OrderBy(o => o.CustomOrderPosition)
        .ToList()
        .AsReadOnly();

    public override CustomFieldType Type => CustomFieldType.SingleSelect;

    public static SingleSelectCustomFieldSetupAggregate Create(
        List<SingleSelectOption> options,
        Guid? defaultOptionId,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project)
    {
        var setup = new SingleSelectCustomFieldSetupAggregate(
            options,
            defaultOptionId,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace);

        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        Guid? defaultOptionId,
        Name name,
        Description description)
    {
        DefaultOptionId = defaultOptionId;
        base.Update(name, description);
    }

    public void AddOption(SingleSelectOption option)
    {
        _options.Add(option);
    }

    public void RemoveOption(SingleSelectOption option)
    {
        for (var i = option.CustomOrderPosition.Value + 1; i < _options.Count; i++)
        {
            Options[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
        }
        _options.Remove(option);
        if (DefaultOptionId == option.Id) DefaultOptionId = null;
        AddDomainEvent(new RemovedOptionFromSingleSelectCustomFieldSetup(this, option));
    }

    public void MoveOption(SingleSelectOption option, SingleSelectOption overOption)
    {
        var newCustomOrderPosition = CustomOrderPosition.Create(overOption.CustomOrderPosition.Value);

        if (option.CustomOrderPosition < overOption.CustomOrderPosition)
        {
            // option is placed on upper position
            for (var pos = option.CustomOrderPosition + 1; pos <= overOption.CustomOrderPosition; pos++)
            {
                Options[pos.Value].UpdateCustomOrderPosition(pos - 1);
            }
        }
        else
        {
            // option is placed on lower position
            for (var pos = overOption.CustomOrderPosition; pos < option.CustomOrderPosition; pos++)
            {
                Options[pos.Value].UpdateCustomOrderPosition(pos + 1);
            }
        }

        option.UpdateCustomOrderPosition(newCustomOrderPosition);
    }
}