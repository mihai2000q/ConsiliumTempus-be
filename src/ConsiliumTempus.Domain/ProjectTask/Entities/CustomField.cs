using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public abstract class CustomField : Entity<CustomFieldId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    protected CustomField()
    {
    }

    protected CustomField(CustomFieldId id, ProjectTaskAggregate projectTask) : base(id)
    {
        ProjectTask = projectTask;
    }

    public ProjectTaskAggregate ProjectTask { get; init; } = null!;
    public abstract CustomFieldSetupAggregate Setup { get; }

    public static CustomField Create(CustomFieldSetupAggregate setup, ProjectTaskAggregate projectTask)
    {
        return setup switch
        {
            NumberCustomFieldSetupAggregate numberSetup =>
                NumberCustomField.Create(
                    numberSetup.DefaultNumber?.Copy(), 
                    numberSetup,
                    projectTask),
            SingleSelectCustomFieldSetupAggregate singleSelectSetup =>
                SingleSelectCustomField.Create(
                    singleSelectSetup.DefaultOption,
                    singleSelectSetup,
                    projectTask),
            TextCustomFieldSetupAggregate textSetup =>
                TextCustomField.Create(
                    textSetup.DefaultText?.Copy(), 
                    textSetup,
                    projectTask),
            _ => throw new ArgumentOutOfRangeException(nameof(setup), setup, "Type Not Supported")
        };
    }
}