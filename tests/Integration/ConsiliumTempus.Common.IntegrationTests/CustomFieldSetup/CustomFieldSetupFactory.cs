using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

public static class CustomFieldSetupFactory
{
    public static DateCustomFieldSetupAggregate CreateDate(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        DateOnly? defaultDate = null)
    {
        return EntityBuilder<DateCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(DateCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(DateCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(DateCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(DateCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(DateCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(TextCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithProperty(nameof(DateCustomFieldSetupAggregate.DefaultDate), defaultDate)
            .Build();
    }

    public static DateTimeCustomFieldSetupAggregate CreateDateTime(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        DateTime? defaultDateTime = null)
    {
        return EntityBuilder<DateTimeCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(DateTimeCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(DateTimeCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(DateTimeCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(DateTimeCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(DateTimeCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(DateTimeCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithProperty(nameof(DateTimeCustomFieldSetupAggregate.DefaultDateTime), defaultDateTime)
            .Build();
    }

    public static DurationCustomFieldSetupAggregate CreateDuration(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        TimeSpan? defaultDuration = null)
    {
        return EntityBuilder<DurationCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(DurationCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(DurationCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(DurationCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(DurationCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(DurationCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(DurationCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithProperty(nameof(DurationCustomFieldSetupAggregate.DefaultDuration), defaultDuration)
            .Build();
    }

    public static MultiSelectCustomFieldSetupAggregate CreateMultiSelect(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        List<MultiSelectOption> options,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description)
    {
        return EntityBuilder<MultiSelectCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(MultiSelectCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(MultiSelectCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(MultiSelectCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(MultiSelectCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(MultiSelectCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(MultiSelectCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithField(nameof(MultiSelectCustomFieldSetupAggregate.Options).ToBackingField(), options)
            .Build();
    }

    public static NumberCustomFieldSetupAggregate CreateNumber(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        NumberCustomFieldSettings? settings = null,
        decimal? defaultNumber = null)
    {
        settings ??= NumberCustomFieldSettings.Create(
            Constants.CustomFieldSetup.CurrencyCode,
            Constants.CustomFieldSetup.Decimals,
            true);

        return EntityBuilder<NumberCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(NumberCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Settings), settings)
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.DefaultNumber),
                defaultNumber.IfNotNull(DecimalNumber.Create))
            .Build();
    }

    public static PeopleCustomFieldSetupAggregate CreatePeople(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description)
    {
        return EntityBuilder<PeopleCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(PeopleCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(PeopleCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(PeopleCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(PeopleCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(PeopleCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(PeopleCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .Build();
    }

    public static SingleSelectCustomFieldSetupAggregate CreateSingleSelect(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        List<SingleSelectOption> options,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        SingleSelectOption? defaultOption = null)
    {
        return EntityBuilder<SingleSelectCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(SingleSelectCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithField(nameof(SingleSelectCustomFieldSetupAggregate.Options).ToBackingField(), options)
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.DefaultOption), defaultOption)
            .Build();
    }

    public static TextCustomFieldSetupAggregate CreateText(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        string? defaultText = null)
    {
        return EntityBuilder<TextCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(TextCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithProperty(nameof(TextCustomFieldSetupAggregate.DefaultText), defaultText.IfNotNull(Text.Create))
            .Build();
    }

    public static TimeCustomFieldSetupAggregate CreateTime(
        WorkspaceAggregate? workspace,
        List<ProjectAggregate> projects,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        TimeOnly? defaultTime = null)
    {
        return EntityBuilder<TimeCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(TimeCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(TimeCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(TimeCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(TimeCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(TimeCustomFieldSetupAggregate.Workspace), workspace)
            .WithField(nameof(TimeCustomFieldSetupAggregate.Projects).ToBackingField(), projects)
            .WithProperty(nameof(TimeCustomFieldSetupAggregate.DefaultTime), defaultTime)
            .Build();
    }
}