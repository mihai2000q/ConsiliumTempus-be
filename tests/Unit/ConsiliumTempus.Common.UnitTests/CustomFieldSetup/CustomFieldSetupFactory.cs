using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupFactory
{
    public static TextCustomFieldSetupAggregate CreateText(
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = TextCustomFieldSetupAggregate.Create(
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
        WorkspaceAggregate? workspace = null,
        ProjectAggregate? project = null,
        UserAggregate? createdBy = null)
    {
        var setup = SingleSelectCustomFieldSetupAggregate.Create(
            options ??
            [
                SingleSelectOption.Create(
                    Constants.SingleSelectOption.Value1,
                    Constants.SingleSelectOption.Color,
                    0),
                SingleSelectOption.Create(
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

    public static List<CustomFieldSetupAggregate> CreateList(
        int count = 5)
    {
        var random = new Random();

        return Enumerable
            .Range(0, count)
            .Select(_ =>
            {
                var randomNumber = random.Next(3);
                return randomNumber switch
                {
                    1 => CreateText() as CustomFieldSetupAggregate,
                    2 => CreateNumber(),
                    _ => CreateSingleSelect()
                };
            })
            .ToList();
    }
}