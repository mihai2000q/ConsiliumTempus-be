using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Variants;

public sealed class MultiSelectCustomFieldSetupAggregate : CustomFieldSetupAggregate
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private MultiSelectCustomFieldSetupAggregate()
    {
    }

    private MultiSelectCustomFieldSetupAggregate(
        List<MultiSelectOption> options,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
    {
        _options = options;
    }

    private readonly List<MultiSelectOption> _options = [];

    public IReadOnlyList<MultiSelectOption> Options => _options
        .OrderBy(o => o.CustomOrderPosition)
        .ToList()
        .AsReadOnly();

    public static MultiSelectCustomFieldSetupAggregate Create(
        List<MultiSelectOption> options,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new MultiSelectCustomFieldSetupAggregate(
            options,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));

        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void AddOption(MultiSelectOption option)
    {
        _options.Add(option);
    }

    public void RemoveOption(MultiSelectOption option)
    {
        for (var i = option.CustomOrderPosition.Value + 1; i < _options.Count; i++)
        {
            Options[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
        }
        _options.Remove(option);
    }

    public void MoveOption(MultiSelectOption option, MultiSelectOption overOption)
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