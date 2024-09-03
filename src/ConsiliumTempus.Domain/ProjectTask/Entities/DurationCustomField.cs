using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class DurationCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private DurationCustomField()
    {
    }

    private DurationCustomField(
        TimeSpan? duration,
        DurationCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        Duration = duration;
        Setup = setup;
    }

    public TimeSpan? Duration { get; private set; }
    public override DurationCustomFieldSetupAggregate Setup { get; } = null!;

    public static DurationCustomField Create(
        TimeSpan? duration,
        DurationCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new DurationCustomField(
            duration,
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }

    public void Update(TimeSpan? duration)
    {
        Duration = duration;
    }
}