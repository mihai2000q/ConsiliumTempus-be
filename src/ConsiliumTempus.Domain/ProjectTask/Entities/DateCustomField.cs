using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectTask.Entities;

public sealed class DateCustomField : CustomField
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private DateCustomField()
    {
    }

    private DateCustomField(
        DateOnly? date,
        DateCustomFieldSetupAggregate setup,
        CustomFieldId id,
        ProjectTaskAggregate projectTask) : base(id, projectTask)
    {
        Date = date;
        Setup = setup;
    }

    public DateOnly? Date { get; private set; }
    public override DateCustomFieldSetupAggregate Setup { get; } = null!;

    public static DateCustomField Create(
        DateOnly? date,
        DateCustomFieldSetupAggregate setup,
        ProjectTaskAggregate projectTask)
    {
        return new DateCustomField(
            date,
            setup,
            CustomFieldId.CreateUnique(),
            projectTask);
    }
    
    public void Update(DateOnly? date)
    {
        Date = date;
    }
}