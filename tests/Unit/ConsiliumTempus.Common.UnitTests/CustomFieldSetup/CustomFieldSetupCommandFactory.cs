using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Common.UnitTests.CustomFieldSetup;

public static class CustomFieldSetupCommandFactory
{
    public static CreateCustomFieldSetupCommand CreateCreateCustomFieldSetupCommand(
        Guid? workspaceId = null,
        Guid? projectId = null,
        string name = Constants.CustomFieldSetup.Name,
        string description = Constants.CustomFieldSetup.Description,
        CustomFieldType type = CustomFieldType.Text,
        CreateCustomFieldSetupCommand.NumberSettingsCommand? numberSettings = null,
        List<CreateCustomFieldSetupCommand.SingleSelectOptionCommand>? singleSelectOptions = null)
    {
        return new CreateCustomFieldSetupCommand(
            workspaceId,
            projectId,
            name,
            description,
            type.ToString(),
            numberSettings,
            singleSelectOptions);
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
            null);
    }
}