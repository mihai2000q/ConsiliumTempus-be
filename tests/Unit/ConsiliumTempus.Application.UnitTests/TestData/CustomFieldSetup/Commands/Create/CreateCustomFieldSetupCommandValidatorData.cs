using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Create;

internal static class CreateCustomFieldSetupCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateCustomFieldSetupCommand>
    {
        public GetValidCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(Guid.NewGuid());
            Add(command);

            command = new CreateCustomFieldSetupCommand(
                Guid.NewGuid(),
                null,
                "New CustomFieldSetup",
                "This field will represent some notes",
                CustomFieldType.Text.ToString().ToLower(),
                null,
                null);
            Add(command);
        }
    }

    internal class GetInvalidWorkspaceIdAndProjectIdCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidWorkspaceIdAndProjectIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand();
            Add(command, nameof(command.WorkspaceId).Dot(nameof(command.ProjectId)));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                name: "");
            Add(command, nameof(command.Name));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                name: new string('a', PropertiesValidation.CustomFieldSetup.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }

    internal class GetInvalidTypeCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidTypeCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommandWithType("");
            Add(command, nameof(command.Type));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommandWithType("NotAType");
            Add(command, nameof(command.Type));
        }
    }

    internal class GetInvalidNumberSettingsCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidNumberSettingsCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: null);
            Add(command, nameof(command.NumberSettings));

            // Currency Code
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "",
                    0,
                    true));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "usd",
                    0,
                    false));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "USd",
                    0,
                    true));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "USdd",
                    0,
                    true));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "USDd",
                    0,
                    true));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "USDD",
                    0,
                    true));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.CurrencyCode)));

            // Decimals
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "USD",
                    -1,
                    true));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.Decimals)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "USD",
                    10,
                    true));
            Add(command, nameof(command.NumberSettings).Dot(nameof(command.NumberSettings.Decimals)));
        }
    }

    internal class GetInvalidSingleSelectOptionsCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidSingleSelectOptionsCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions: null);
            Add(command, nameof(command.SingleSelectOptions));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions: []);
            Add(command, nameof(command.SingleSelectOptions));

            // Value
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "",
                        "#22FF22")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Value)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        new string('a', PropertiesValidation.SingleSelectOption.ValueMaximumLength + 1),
                        "#22FF22")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Value)));

            // Color
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "High",
                        "")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "High",
                        "not a color")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "High",
                        "FF2233")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "High",
                        "#FF223")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "High",
                        "#GG2233")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions:
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "High",
                        "#FF2233"),
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "",
                        "#11EE33")
                ]);
            Add(command, nameof(command.SingleSelectOptions) + "[1]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectOptionCommand.Value)));
        }
    }
}