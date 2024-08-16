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
                null,
                new CreateCustomFieldSetupCommand.CreateTextCustomFieldSetupCommand(null));
            Add(command);
        }
    }

    internal class GetInvalidWorkspaceIdAndProjectIdCommands : TheoryData<CreateCustomFieldSetupCommand, string>
    {
        public GetInvalidWorkspaceIdAndProjectIdCommands()
        {
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand();
            Add(command, nameof(command.WorkspaceId).Dot(nameof(command.ProjectId)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                Guid.Empty,
                Guid.Empty);
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
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "usd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USdd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USDd",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USDD",
                        0,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));

            // Decimals
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        -1,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        PropertiesValidation.CustomFieldSetup.Number.DecimalsMaximum + 1,
                        true),
                    null));
            Add(command, nameof(command.NumberCustomFieldSetup)
                .Dot(nameof(command.NumberCustomFieldSetup.Settings)));
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
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.Options)));

            // Id
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "",
                                    "Some Value",
                                    "#22FF22")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Id)));

            // Value
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "",
                                    "#22FF22")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Value)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    new string('a', PropertiesValidation.SingleSelectOption.ValueMaximumLength + 1),
                                    "#22FF22")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Value)));

            // Color
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "not a color")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "FF2233")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#F2233")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#GG2233")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[0]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Color)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#FF2233"),
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "2",
                                    "",
                                    "#11EE33")
                        ],
                        null));
            Add(command, nameof(command.SingleSelectCustomFieldSetup) + "[1]"
                .Dot(nameof(CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand
                    .SingleSelectOptionCommand.Value)));
            
            // Default Option Id
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#FF2233"),
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "2",
                                    "Low",
                                    "#11EE33")
                        ],
                        "3"));
            Add(command, nameof(command.SingleSelectCustomFieldSetup)
                .Dot(nameof(command.SingleSelectCustomFieldSetup.DefaultOptionId)));

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new
                    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                        [
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "High",
                                    "#FF2233"),
                            new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.
                                SingleSelectOptionCommand(
                                    "1",
                                    "Low",
                                    "#11EE33")
                        ],
                        "1"));
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