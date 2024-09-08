using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupFactory
{
    public static DateCustomFieldSetupAggregate CreateDate(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        DateOnly? defaultDate = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = DateCustomFieldSetupAggregate.Create(
            defaultDate,
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static DateTimeCustomFieldSetupAggregate CreateDateTime(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        DateTime? defaultDateTime = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = DateTimeCustomFieldSetupAggregate.Create(
            defaultDateTime,
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static DurationCustomFieldSetupAggregate CreateDuration(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        TimeSpan? defaultDuration = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = DurationCustomFieldSetupAggregate.Create(
            defaultDuration,
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static MultiSelectCustomFieldSetupAggregate CreateMultiSelect(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        List<MultiSelectOption>? options = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = MultiSelectCustomFieldSetupAggregate.Create(
            options ??
            [
                MultiSelectOptionFactory.Create(),
                MultiSelectOptionFactory.Create(
                    Constants.SingleSelectOption.Value2,
                    Constants.SingleSelectOption.Color,
                    1)
            ],
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static NumberCustomFieldSetupAggregate CreateNumber(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        decimal? defaultNumber = null,
        NumberCustomFieldSettings? numberSettings = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = NumberCustomFieldSetupAggregate.Create(
            numberSettings ?? NumberCustomFieldSettings.Create(
                Constants.CustomFieldSetup.CurrencyCode,
                Constants.CustomFieldSetup.Decimals,
                true),
            defaultNumber.IfNotNull(DecimalNumber.Create),
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static PeopleCustomFieldSetupAggregate CreatePeople(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = PeopleCustomFieldSetupAggregate.Create(
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static SingleSelectCustomFieldSetupAggregate CreateSingleSelect(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        List<SingleSelectOption>? options = null,
        Guid? defaultOptionId = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = SingleSelectCustomFieldSetupAggregate.Create(
            options ??
            [
                SingleSelectOptionFactory.Create(),
                SingleSelectOptionFactory.Create(
                    Constants.SingleSelectOption.Value2,
                    Constants.SingleSelectOption.Color,
                    1)
            ],
            defaultOptionId,
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static TextCustomFieldSetupAggregate CreateText(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        string? defaultText = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = TextCustomFieldSetupAggregate.Create(
            defaultText.IfNotNull(Text.Create),
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static TimeCustomFieldSetupAggregate CreateTime(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        TimeOnly? defaultTime = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = TimeCustomFieldSetupAggregate.Create(
            defaultTime,
            Name.Create(name),
            Description.Create(description),
            workspace,
            project,
            createdBy ?? UserFactory.Create());

        setup.ClearDomainEvents();

        return setup;
    }

    public static List<CustomFieldSetupAggregate> CreateList()
    {
        return
        [
            CreateDate(),
            CreateDateTime(),
            CreateDuration(),
            CreateMultiSelect(),
            CreateNumber(),
            CreatePeople(),
            CreateSingleSelect(),
            CreateText(),
            CreateTime()
        ];
    }
}