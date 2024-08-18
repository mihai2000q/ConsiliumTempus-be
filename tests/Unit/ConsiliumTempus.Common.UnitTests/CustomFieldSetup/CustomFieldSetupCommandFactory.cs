using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.UpdateWorkspace;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupCommandFactory
{
    public static AddCustomFieldSetupToProjectCommand CreateAddCustomFieldSetupToProjectCommand(
        Guid? id = null,
        Guid? projectId = null)
    {
        return new AddCustomFieldSetupToProjectCommand(
            id ?? Guid.NewGuid(),
            projectId ?? Guid.NewGuid());
    }

    public static CreateCustomFieldSetupCommand CreateCreateCustomFieldSetupCommand(
        Guid? workspaceId = null,
        Guid? projectId = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand? numberCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand? singleSelectCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.CreateTextCustomFieldSetupCommand? textCustomFieldSetup = null)
    {
        return new CreateCustomFieldSetupCommand(
            workspaceId,
            projectId,
            name,
            description,
            type.ToString(),
            numberCustomFieldSetup,
            singleSelectCustomFieldSetup,
            textCustomFieldSetup);
    }

    public static CreateCustomFieldSetupCommand CreateCreateCustomFieldSetupCommandWithType(string type)
    {
        return new CreateCustomFieldSetupCommand(
            Guid.NewGuid(),
            null,
            Constants.CustomFieldSetup.Name,
            Constants.CustomFieldSetup.Description,
            type,
            null,
            null,
            new CreateCustomFieldSetupCommand.CreateTextCustomFieldSetupCommand(null));
    }

    public static DeleteCustomFieldSetupCommand CreateDeleteCustomFieldSetupCommand(Guid? id = null)
    {
        return new DeleteCustomFieldSetupCommand(id ?? Guid.NewGuid());
    }

    public static RemoveCustomFieldSetupFromProjectCommand CreateRemoveCustomFieldSetupFromProjectCommand(
        Guid? id = null,
        Guid? projectId = null)
    {
        return new RemoveCustomFieldSetupFromProjectCommand(
            id ?? Guid.NewGuid(),
            projectId ?? Guid.NewGuid());
    }

    public static UpdateWorkspaceCustomFieldSetupCommand CreateUpdateWorkspaceCustomFieldSetupCommand(
        Guid? id = null,
        Guid? workspaceId = null)
    {
        return new UpdateWorkspaceCustomFieldSetupCommand(
            id ?? Guid.NewGuid(),
            workspaceId ?? Guid.NewGuid());
    }
}