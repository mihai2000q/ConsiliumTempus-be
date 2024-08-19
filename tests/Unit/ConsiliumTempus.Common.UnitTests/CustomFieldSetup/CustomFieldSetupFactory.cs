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

    public static SingleSelectCustomFieldSetupAggregate CreateSingleSelect(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        List<SingleSelectOption>? options = null,
        SingleSelectOption? defaultOption = null,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = SingleSelectCustomFieldSetupAggregate.Create(
            options ??
            [
                SingleSelectOptionFactory.Create(),
                SingleSelectOption.Create(
                    Constants.SingleSelectOption.Value2,
                    Constants.SingleSelectOption.Color,
                    CustomOrderPosition.Create(1))
            ],
            defaultOption,
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

    public static List<CustomFieldSetupAggregate> CreateList()
    {
        return 
        [
            CreateNumber(),
            CreateSingleSelect(),
            CreateText()
        ];
    }
}