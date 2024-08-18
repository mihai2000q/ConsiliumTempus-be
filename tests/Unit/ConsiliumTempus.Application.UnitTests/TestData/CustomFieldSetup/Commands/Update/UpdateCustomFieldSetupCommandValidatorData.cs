using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Update;

internal static class UpdateCustomFieldSetupCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateCustomFieldSetupCommand>
    {
        public GetValidCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                textCustomFieldSetup: new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command);

            command = new UpdateCustomFieldSetupCommand(
                Guid.NewGuid(),
                "New CustomFieldSetup",
                "This field will represent some notes",
                CustomFieldType.Text.ToString(),
                null,
                null,
                new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand("Default Text"));
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: Guid.Empty,
                textCustomFieldSetup: new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                name: "",
                textCustomFieldSetup: new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, nameof(command.Name));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                name: new string('a', PropertiesValidation.CustomFieldSetup.NameMaximumLength + 1),
                textCustomFieldSetup: new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, nameof(command.Name));
        }
    }

    internal class GetInvalidTypeCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidTypeCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommandWithType("");
            Add(command, nameof(command.Type));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommandWithType("NotAType");
            Add(command, nameof(command.Type));
        }
    }

    internal class GetInvalidNumberCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidNumberCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: null);
            Add(command, nameof(command.NumberCustomFieldSetup));

            // Currency Code
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "usd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USdd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USDd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USDD",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            // Decimals
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        -1,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.Decimals)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum + 1,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.Decimals)));
        }
    }

    internal class GetInvalidSingleSelectCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidSingleSelectCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: null);
            Add(command, nameof(command.SingleSelectCustomFieldSetup));
        }
    }

    internal class GetInvalidTextCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidTextCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                textCustomFieldSetup: null);
            Add(command, nameof(command.TextCustomFieldSetup));
        }
    }
}