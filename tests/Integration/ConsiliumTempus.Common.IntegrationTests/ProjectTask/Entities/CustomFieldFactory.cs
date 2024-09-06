using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Common.IntegrationTests.ProjectTask.Entities;

public static class CustomFieldFactory
{
    public static DateCustomField CreateDate(
        ProjectTaskAggregate projectTask,
        DateCustomFieldSetupAggregate customFieldSetup,
        DateOnly? date = null)
    {
        return EntityBuilder<DateCustomField>.Empty()
            .WithProperty(nameof(DateCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(DateCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(DateCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(DateCustomField.Date), date)
            .Build();
    }
    
    public static DateTimeCustomField CreateDateTime(
        ProjectTaskAggregate projectTask,
        DateTimeCustomFieldSetupAggregate customFieldSetup,
        DateTime? dateTime = null)
    {
        return EntityBuilder<DateTimeCustomField>.Empty()
            .WithProperty(nameof(DateTimeCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(DateTimeCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(DateTimeCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(DateTimeCustomField.DateTime), dateTime)
            .Build();
    }
    
    public static DurationCustomField CreateDuration(
        ProjectTaskAggregate projectTask,
        DurationCustomFieldSetupAggregate customFieldSetup,
        TimeSpan? duration = null)
    {
        return EntityBuilder<DurationCustomField>.Empty()
            .WithProperty(nameof(DurationCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(DurationCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(DurationCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(DurationCustomField.Duration), duration)
            .Build();
    }
    
    public static MultiSelectCustomField CreateMultiSelect(
        ProjectTaskAggregate projectTask,
        MultiSelectCustomFieldSetupAggregate customFieldSetup,
        List<MultiSelectOption>? options = null)
    {
        return EntityBuilder<MultiSelectCustomField>.Empty()
            .WithProperty(nameof(MultiSelectCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(MultiSelectCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(MultiSelectCustomField.ProjectTask), projectTask)
            .WithField(nameof(MultiSelectCustomField.Options).ToBackingField(), options ?? [])
            .Build();
    }
    
    public static NumberCustomField CreateNumber(
        ProjectTaskAggregate projectTask,
        NumberCustomFieldSetupAggregate customFieldSetup,
        decimal? number = null)
    {
        return EntityBuilder<NumberCustomField>.Empty()
            .WithProperty(nameof(NumberCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(NumberCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(NumberCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(NumberCustomField.Number), number.IfNotNull(DecimalNumber.Create))
            .Build();
    }
    
    public static PeopleCustomField CreatePeople(
        ProjectTaskAggregate projectTask,
        PeopleCustomFieldSetupAggregate customFieldSetup,
        UserAggregate? person = null)
    {
        return EntityBuilder<PeopleCustomField>.Empty()
            .WithProperty(nameof(PeopleCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(PeopleCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(PeopleCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(PeopleCustomField.Person), person)
            .Build();
    }

    public static SingleSelectCustomField CreateSingleSelect(
        ProjectTaskAggregate projectTask,
        SingleSelectCustomFieldSetupAggregate customFieldSetup,
        SingleSelectOption? option = null)
    {
        return EntityBuilder<SingleSelectCustomField>.Empty()
            .WithProperty(nameof(SingleSelectCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(SingleSelectCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(SingleSelectCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(SingleSelectCustomField.Option), option)
            .Build();
    }

    public static TextCustomField CreateText(
        ProjectTaskAggregate projectTask,
        TextCustomFieldSetupAggregate customFieldSetup,
        string? text = null)
    {
        return EntityBuilder<TextCustomField>.Empty()
            .WithProperty(nameof(TextCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(TextCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(TextCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(TextCustomField.Text), text.IfNotNull(Text.Create))
            .Build();
    }
    
    public static TimeCustomField CreateTime(
        ProjectTaskAggregate projectTask,
        TimeCustomFieldSetupAggregate customFieldSetup,
        TimeOnly? time = null)
    {
        return EntityBuilder<TimeCustomField>.Empty()
            .WithProperty(nameof(TimeCustomField.Id), CustomFieldId.CreateUnique())
            .WithProperty(nameof(TimeCustomField.ProjectTask), projectTask)
            .WithProperty(nameof(TimeCustomField.Setup), customFieldSetup)
            .WithProperty(nameof(TimeCustomField.Time), time)
            .Build();
    }
}