using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class DateTimeCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private DateTimeCustomField()
    {
    }

    private DateTimeCustomField(
        DateTime? dateTime,
        DateTimeCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        DateTime = dateTime;
        Setup = setup;
    }

    public DateTime? DateTime { get; private set; }
    public override DateTimeCustomFieldSetupAggregate Setup { get; } = null!;

    public static DateTimeCustomField Create(
        DateTime? dateTime,
        DateTimeCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new DateTimeCustomField(
            dateTime,
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }
    
    public void UpDateTime(DateTime? dateTime)
    {
        DateTime = dateTime;
    }
}