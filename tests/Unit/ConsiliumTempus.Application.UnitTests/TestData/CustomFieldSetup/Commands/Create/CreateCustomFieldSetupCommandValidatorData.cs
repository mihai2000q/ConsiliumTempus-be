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
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                Guid.NewGuid(),
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command);

            command = new CreateCustomFieldSetupCommand(
                Guid.NewGuid(),
                null,
                "New CustomFieldSetup",
                "This field will represent some notes",
                CustomFieldType.Text,
                null,
                null,
                new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand("Default Text"));
            Add(command);
        }
    }

    internal class GetInvalidWorkspaceIdAndProjectIdCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidWorkspaceIdAndProjectIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, nameof(command.WorkspaceId).Dot(nameof(command.ProjectId)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                Guid.Empty,
                Guid.Empty,
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, nameof(command.WorkspaceId).Dot(nameof(command.ProjectId)));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                name: "",
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, nameof(command.Name));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                name: new string('a', PropertiesValidation.CustomFieldSetup.NameMaximumLength + 1),
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, nameof(command.Name));
        }
    }

    internal class GetInvalidNumberCustomFieldSetupCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidNumberCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: null);
            Add(command, nameof(command.NumberCustomFieldSetup));

            // Currency Code
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "usd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USdd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USDd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USDD",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.CurrencyCode)));

            // Decimals
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        -1,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.Decimals)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum + 1,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings))
                .Dot(nameof(command.NumberCustomFieldSetup.Settings.Decimals)));
        }
    }

    internal class GetInvalidSingleSelectCustomFieldSetupCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidSingleSelectCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: null);
            Add(command, nameof(command.SingleSelectCustomFieldSetup));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)));

            // Id
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "",
                                    "Some Value",
                                    "#22FF22")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Id)));

            // Value
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "",
                                    "#22FF22")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Value)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    new string('a', PropertiesValidation.SingleSelectOption.ValueMaximumLength + 1),
                                    "#22FF22")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Value)));

            // Color
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "not a color")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "FF2233")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#F2233")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#GG2233")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#FF2233"),
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "2",
                                    "",
                                    "#11EE33")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)) + "[1]"
                .Dot(nameof(CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Value)));
            
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#FF2233"),
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "Low",
                                    "#11EE33")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)));
            
            // Default Option Id
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#FF2233"),
                            new CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "2",
                                    "Low",
                                    "#11EE33")
                        ],
                        "3"));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.DefaultOptionId)));
        }
    }
    
    internal class GetInvalidTextCustomFieldSetupCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidTextCustomFieldSetupCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                textCustomFieldSetup: null);
            Add(command, nameof(command.TextCustomFieldSetup));
        }
    }
}