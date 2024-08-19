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
            // Number
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        2,
                        true),
                    null));
            Add(command);

            // Single Select
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#2233FF",
                        "Low"),
                    null,
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Update.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#2233FF",
                        "Low"),
                    Guid.NewGuid(),
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move.ToString(),
                    null,
                    Guid.NewGuid(),
                    Guid.NewGuid()));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Remove.ToString(),
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command);

            // Text
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
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

    internal class
        GetInvalidSingleSelectCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidSingleSelectCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: null);
            Add(command, nameof(command.SingleSelectCustomFieldSetup));

            // Operations
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    null,
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Update.ToString(),
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption)));
            
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Update.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#FF1122",
                        "Something"),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.OptionId)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move.ToString(),
                    null,
                    Guid.NewGuid(), 
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.OverOptionId)));
            
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move.ToString(),
                    null,
                    null, 
                    Guid.NewGuid()));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.OptionId)));
            
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Remove.ToString(),
                    null,
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.OptionId)));

            // New Option: Value
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#FF1122",
                        ""),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption.Value)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#FF1122",
                        new string('a', PropertiesValidation.SingleSelectOption.ValueMaximumLength + 1)),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption.Value)));

            // New Option: Color
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "not a color",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "FF2233",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#F2233",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add.ToString(),
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#GG2233",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption.Color)));
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