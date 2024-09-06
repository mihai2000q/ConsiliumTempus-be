using ConsiliumTempus.Application.CustomFieldSetup.Commands.AddToProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Delete;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.MakeGlobal;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.RemoveFromProject;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
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
        CreateCustomFieldSetupCommand.DateCustomFieldSetupCommand? dateCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand? dateTimeCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.DurationCustomFieldSetupCommand? durationCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand? multiSelectCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? numberCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? singleSelectCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand? textCustomFieldSetup = null,
        CreateCustomFieldSetupCommand.TimeCustomFieldSetupCommand? timeCustomFieldSetup = null)
    {
        return new CreateCustomFieldSetupCommand(
            workspaceId,
            projectId,
            name,
            description,
            type,
            dateCustomFieldSetup,
            dateTimeCustomFieldSetup,
            durationCustomFieldSetup,
            multiSelectCustomFieldSetup,
            numberCustomFieldSetup,
            singleSelectCustomFieldSetup,
            textCustomFieldSetup,
            timeCustomFieldSetup);
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

    public static UpdateCustomFieldSetupCommand CreateUpdateCustomFieldSetupCommand(
        Guid? id = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        UpdateCustomFieldSetupCommand.DateCustomFieldSetupCommand? dateCustomFieldSetup = null,
        UpdateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand? dateTimeCustomFieldSetup = null,
        UpdateCustomFieldSetupCommand.DurationCustomFieldSetupCommand? durationCustomFieldSetup = null,
        UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand? multiSelectCustomFieldSetup = null,
        UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? numberCustomFieldSetup = null,
        UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? singleSelectCustomFieldSetup = null,
        UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand? textCustomFieldSetup = null,
        UpdateCustomFieldSetupCommand.TimeCustomFieldSetupCommand? timeCustomFieldSetup = null)
    {
        return new UpdateCustomFieldSetupCommand(
            id ?? Guid.NewGuid(),
            name,
            description,
            type,
            dateCustomFieldSetup,
            dateTimeCustomFieldSetup,
            durationCustomFieldSetup,
            multiSelectCustomFieldSetup,
            numberCustomFieldSetup,
            singleSelectCustomFieldSetup,
            textCustomFieldSetup,
            timeCustomFieldSetup);
    }

    public static MakeCustomFieldSetupGlobalCommand CreateMakeCustomFieldSetupGlobalCommand(Guid? id = null)
    {
        return new MakeCustomFieldSetupGlobalCommand(id ?? Guid.NewGuid());
    }
}