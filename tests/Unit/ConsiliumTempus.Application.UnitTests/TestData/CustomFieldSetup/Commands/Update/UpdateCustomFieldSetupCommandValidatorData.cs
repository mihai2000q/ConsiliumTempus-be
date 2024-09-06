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
            // Date
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new UpdateCustomFieldSetupCommand.DateCustomFieldSetupCommand(null));
            Add(command);

            // Date Time
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new UpdateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand(null));
            Add(command);

            // Duration
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new UpdateCustomFieldSetupCommand.DurationCustomFieldSetupCommand(null));
            Add(command);

            // Multi Select
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    null,
                    null,
                    null,
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#2233FF",
                        "Low"),
                    null,
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#2233FF",
                        "Low"),
                    Guid.NewGuid(),
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    Guid.NewGuid(),
                    Guid.NewGuid()));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Remove,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command);

            // Number
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        2,
                        true),
                    null));
            Add(command);

            // People
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.People);
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    Guid.NewGuid(),
                    Guid.NewGuid()));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.OptionOperation.Remove,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command);

            // Text
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                textCustomFieldSetup: new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command);

            // Time
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new UpdateCustomFieldSetupCommand.TimeCustomFieldSetupCommand(null));
            Add(command);

            command = new UpdateCustomFieldSetupCommand(
                Guid.NewGuid(),
                "New TextCustomFieldSetup",
                "This field will represent some notes",
                CustomFieldType.Text,
                null,
                null,
                null,
                null,
                null,
                null,
                new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand("Default Text"),
                null);
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

    internal class GetInvalidDateCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidDateCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Date,
                dateCustomFieldSetup: null);
            Add(command, nameof(command.DateCustomFieldSetup));
        }
    }

    internal class GetInvalidDateTimeCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidDateTimeCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: null);
            Add(command, nameof(command.DateTimeCustomFieldSetup));
        }
    }

    internal class GetInvalidDurationCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidDurationCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: null);
            Add(command, nameof(command.DurationCustomFieldSetup));
        }
    }
    
    internal class
        GetInvalidMultiSelectCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidMultiSelectCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: null);
            Add(command, nameof(command.MultiSelectCustomFieldSetup));

            // Operations
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    null,
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#FF1122",
                        "Something"),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.OptionId)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.OverOptionId)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    null,
                    Guid.NewGuid()));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.OptionId)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Remove,
                    null,
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.OptionId)));

            // New Option: Value
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#FF1122",
                        ""),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption.Value)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#FF1122",
                        new string('a', PropertiesValidation.MultiSelectOption.ValueMaximumLength + 1)),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption.Value)));

            // New Option: Color
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "not a color",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "FF2233",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#F2233",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption.Color)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#GG2233",
                        "Low"),
                    null,
                    null));
            Add(command, nameof(command.MultiSelectCustomFieldSetup)
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption))
                .Dot(nameof(command.MultiSelectCustomFieldSetup.NewOption.Color)));
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    null,
                    null,
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.NewOption)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.OverOptionId)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    null,
                    Guid.NewGuid()));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.OptionId)));

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null,
                    UpdateCustomFieldSetupCommand.OptionOperation.Remove,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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

    internal class GetInvalidTimeCustomFieldSetupCommands : TheoryData<UpdateCustomFieldSetupCommand, string>
    {
        public GetInvalidTimeCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                type: CustomFieldType.Time,
                timeCustomFieldSetup: null);
            Add(command, nameof(command.TimeCustomFieldSetup));
        }
    }
}