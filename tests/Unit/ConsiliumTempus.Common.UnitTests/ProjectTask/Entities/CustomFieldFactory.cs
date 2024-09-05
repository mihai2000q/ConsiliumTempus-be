using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;

public static class CustomFieldFactory
{
    public static DateCustomField CreateDate(
        DateOnly? date = null,
        DateCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return DateCustomField.Create(
            date,
            setup ?? CustomFieldSetupFactory.CreateDate(),
            task ?? ProjectTaskFactory.Create());
    }

    public static DateTimeCustomField CreateDateTime(
        DateTime? dateTime = null,
        DateTimeCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return DateTimeCustomField.Create(
            dateTime,
            setup ?? CustomFieldSetupFactory.CreateDateTime(),
            task ?? ProjectTaskFactory.Create());
    }

    public static DurationCustomField CreateDuration(
        TimeSpan? duration = null,
        DurationCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return DurationCustomField.Create(
            duration,
            setup ?? CustomFieldSetupFactory.CreateDuration(),
            task ?? ProjectTaskFactory.Create());
    }

    public static MultiSelectCustomField CreateMultiSelect(
        MultiSelectCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return MultiSelectCustomField.Create(
            setup ?? CustomFieldSetupFactory.CreateMultiSelect(),
            task ?? ProjectTaskFactory.Create());
    }

    public static NumberCustomField CreateNumber(
        decimal? number = null,
        NumberCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return NumberCustomField.Create(
            number.IfNotNull(DecimalNumber.Create),
            setup ?? CustomFieldSetupFactory.CreateNumber(),
            task ?? ProjectTaskFactory.Create());
    }

    public static PeopleCustomField CreatePeople(
        UserAggregate? person = null,
        PeopleCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return PeopleCustomField.Create(
            person,
            setup ?? CustomFieldSetupFactory.CreatePeople(),
            task ?? ProjectTaskFactory.Create());
    }

    public static SingleSelectCustomField CreateSingleSelect(
        Guid? optionId = null,
        SingleSelectCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        setup ??= CustomFieldSetupFactory.CreateSingleSelect();
        var option = setup.Options.SingleOrDefault(o => o.Id == optionId);

        return SingleSelectCustomField.Create(
            option,
            setup,
            task ?? ProjectTaskFactory.Create());
    }

    public static TextCustomField CreateText(
        string? text = null,
        TextCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return TextCustomField.Create(
            text.IfNotNull(Text.Create),
            setup ?? CustomFieldSetupFactory.CreateText(),
            task ?? ProjectTaskFactory.Create());
    }

    public static TimeCustomField CreateTime(
        TimeOnly? time = null,
        TimeCustomFieldSetupAggregate? setup = null,
        ProjectTaskAggregate? task = null)
    {
        return TimeCustomField.Create(
            time,
            setup ?? CustomFieldSetupFactory.CreateTime(),
            task ?? ProjectTaskFactory.Create());
    }
}