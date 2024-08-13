using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Entities;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Infrastructure.Extensions;

namespace ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

public static class CustomFieldSetupFactory
{
    public static NumberCustomFieldSetupAggregate CreateNumber(
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        NumberCustomFieldSettings? settings = null)
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
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Project), project)
            .WithProperty(nameof(NumberCustomFieldSetupAggregate.Settings), settings)
            .Build();
    }
    
    public static SingleSelectCustomFieldSetupAggregate CreateSingleSelect(
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        Audit audit,
        List<SingleSelectOption> options,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description)
    {
        return EntityBuilder<SingleSelectCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Workspace), workspace)
            .WithProperty(nameof(SingleSelectCustomFieldSetupAggregate.Project), project)
            .WithField(nameof(SingleSelectCustomFieldSetupAggregate.Options).ToBackingField(), options)
            .Build();
    }
    
    public static TextCustomFieldSetupAggregate CreateText(
        WorkspaceAggregate? workspace,
        ProjectAggregate? project,
        Audit audit,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description)
    {
        return EntityBuilder<TextCustomFieldSetupAggregate>.Empty()
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Id), CustomFieldSetupId.CreateUnique())
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Name), Name.Create(name))
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Description), Description.Create(description))
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Audit), audit)
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Workspace), workspace)
            .WithProperty(nameof(TextCustomFieldSetupAggregate.Project), project)
            .Build();
    }
}