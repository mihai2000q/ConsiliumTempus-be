using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class TimeCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private TimeCustomField()
    {
    }

    private TimeCustomField(
        TimeOnly? time,
        TimeCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        Time = time;
        Setup = setup;
    }

    public TimeOnly? Time { get; private set; }
    public override TimeCustomFieldSetupAggregate Setup { get; } = null!;

    public static TimeCustomField Create(
        TimeOnly? time,
        TimeCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new TimeCustomField(
            time,
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }

    public void Update(TimeOnly? time)
    {
        Time = time;
    }
}