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
        Guid? defaultOptionId,
        CustomFieldSetupId id,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        Audit audit) : base(id, name, description, workspace, audit)
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

    public static SingleSelectCustomFieldSetupAggregate Create(
        List<SingleSelectOption> options,
        Guid? defaultOptionId,
        Name name,
        Description description,
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        UserAggregate createdBy)
    {
        var setup = new SingleSelectCustomFieldSetupAggregate(
            options,
            defaultOptionId,
            CustomFieldSetupId.CreateUnique(),
            name,
            description,
            workspace,
            Audit.Create(createdBy));

        if (project != null) setup.AddProject(project);

        return setup;
    }

    public void Update(
        Guid? defaultOptionId,
        Name name,
        Description description,
        UserAggregate updatedBy)
    {
        DefaultOptionId = defaultOptionId;
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
            Options[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
        }
        _options.Remove(option);
    }

    public void MoveOption(SingleSelectOption option, SingleSelectOption overOption)
    {
        var newCustomOrderPosition = CustomOrderPosition.Create(overOption.CustomOrderPosition.Value);

        if (option.CustomOrderPosition < overOption.CustomOrderPosition)
        {
            // option is placed on upper position
            for (var pos = option.CustomOrderPosition + 1; pos <= overOption.CustomOrderPosition; pos++)
            {
                Options[pos.Value].UpdateCustomOrderPosition(--pos);
            }
        }
        else
        {
            // option is placed on lower position
            for (var pos = overOption.CustomOrderPosition; pos < option.CustomOrderPosition; pos++)
            {
                Options[pos.Value].UpdateCustomOrderPosition(++pos);
            }
        }

        option.UpdateCustomOrderPosition(newCustomOrderPosition);
    }
}