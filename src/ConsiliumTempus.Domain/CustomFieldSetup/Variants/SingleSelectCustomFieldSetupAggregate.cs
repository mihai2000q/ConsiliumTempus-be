using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
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
        SingleSelectOption? defaultOption,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
    {
        _options = options;
        DefaultOption = defaultOption;
    }

    private readonly List<SingleSelectOption> _options = [];

    public SingleSelectOption? DefaultOption { get; private set; }

    public IReadOnlyList<SingleSelectOption> Options => _options
        .OrderBy(o => o.CustomOrderPosition)
        .ToList()
        .AsReadOnly();

    public static SingleSelectCustomFieldSetupAggregate Create(
        List<SingleSelectOption> options,
        SingleSelectOption? defaultOption,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new SingleSelectCustomFieldSetupAggregate(
            options,
            defaultOption,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));

        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        SingleSelectOption? defaultOption,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        DefaultOption = defaultOption;
        base.Update(name, description, updatedBy);
    }

    public void AddOption(SingleSelectOption option)
    {
        _options.Add(option);
    }

    public void RemoveOption(SingleSelectOption option)
    {
        for (var i = option.CustomOrderPosition.Value + 1; i < _options.Count; i++)
        {
            _options[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
        }
        _options.Remove(option);
    }

    public void MoveOption(SingleSelectOption option, SingleSelectOption overOption)
    {
        var newCustomOrderPosition = CustomOrderPosition.Create(overOption.CustomOrderPosition.Value);

        if (option.CustomOrderPosition < overOption.CustomOrderPosition)
        {
            // stage is placed on upper position
            for (var i = option.CustomOrderPosition.Value + 1; i <= overOption.CustomOrderPosition.Value; i++)
            {
                _options[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
            }
        }
        else
        {
            // stage is placed on lower position
            for (var i = overOption.CustomOrderPosition.Value; i < option.CustomOrderPosition.Value; i++)
            {
                _options[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i + 1));
            }
        }

        option.UpdateCustomOrderPosition(newCustomOrderPosition);
    }
}