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
            DateCustomFieldSetupAggregate dateSetup =>
                DateCustomField.Create(
                    dateSetup.DefaultDate,
                    dateSetup,
                    projectTask),

            DateTimeCustomFieldSetupAggregate dateTimeSetup =>
                DateTimeCustomField.Create(
                    dateTimeSetup.DefaultDateTime,
                    dateTimeSetup,
                    projectTask),

            DurationCustomFieldSetupAggregate durationSetup =>
                DurationCustomField.Create(
                    durationSetup.DefaultDuration,
                    durationSetup,
                    projectTask),

            MultiSelectCustomFieldSetupAggregate multiSelectSetup =>
                MultiSelectCustomField.Create(
                    multiSelectSetup,
                    projectTask),

            NumberCustomFieldSetupAggregate numberSetup =>
                NumberCustomField.Create(
                    numberSetup.DefaultNumber?.Copy(),
                    numberSetup,
                    projectTask),

            PeopleCustomFieldSetupAggregate peopleSetup =>
                PeopleCustomField.Create(
                    null,
                    peopleSetup,
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

            TimeCustomFieldSetupAggregate timeSetup =>
                TimeCustomField.Create(
                    timeSetup.DefaultTime,
                    timeSetup,
                    projectTask),

            _ => throw new ArgumentOutOfRangeException(nameof(setup), setup, "Type Not Supported")
        };
    }
}