using ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Create;

internal static class CreateCustomFieldSetupCommandHandlerData
{
    internal class GetCommands : TheoryData<CreateCustomFieldSetupCommand>
    {
        public GetCommands()
        {
            // Workspace and Project
            var command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                Guid.NewGuid());
            Add(command);
            
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                null,
                Guid.NewGuid());
            Add(command);
            
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);

            // Number
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                    "USD",
                    2,
                    true),
                    null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand(
                    new CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "EUR",
                        3,
                        false),
                    12));
            Add(command);

            // Single Select
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                    [
                        new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "1",
                            "High",
                            "#3322FF"),
                        new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "2",
                            "Medium",
                            "#7722EF"),
                        new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "3",
                            "Low",
                            "#99G244")
                    ],
                    null
                    ));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand(
                    [
                        new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "1",
                            "High",
                            "#3322FF"),
                        new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "2",
                            "Medium",
                            "#7722EF"),
                        new CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                            "3",
                            "Low",
                            "#99G244")
                    ],
                    "1"
                ));
            Add(command);

            // Text
            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateTextCustomFieldSetupCommand(null));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Text,
                textCustomFieldSetup: new CreateCustomFieldSetupCommand.CreateTextCustomFieldSetupCommand("Default Text"));
            Add(command);
        }
    }
}