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

    protected CustomField(CustomFieldId id, ProjectTaskAggregate task) : base(id)
    {
        Task = task;
    }

    public ProjectTaskAggregate Task { get; init; } = null!;

    public static CustomField Create(CustomFieldSetupAggregate setup, ProjectTaskAggregate task)
    {
        return setup switch
        {
            NumberCustomFieldSetupAggregate numberSetup =>
                NumberCustomField.Create(
                    null,
                    numberSetup,
                    task),
            SingleSelectCustomFieldSetupAggregate singleSelectSetup =>
                SingleSelectCustomField.Create(
                    null,
                    singleSelectSetup,
                    task),
            TextCustomFieldSetupAggregate textSetup =>
                TextCustomField.Create(
                    null,
                    textSetup,
                    task),
            _ => throw new ArgumentOutOfRangeException(nameof(setup), setup, "Type Not Supported")
        };
    }
}