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

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.Number,
                numberSettings: new CreateCustomFieldSetupCommand.NumberSettingsCommand(
                    "USD",
                    2,
                    true));
            Add(command);

            command = CustomFieldSetupCommandFactory.CreateCreateCustomFieldSetupCommand(
                projectId: Guid.NewGuid(),
                type: CustomFieldType.SingleSelect,
                singleSelectOptions: 
                [
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "High",
                        "#3322FF"),
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "Medium",
                        "#7722EF"),
                    new CreateCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "Low",
                        "#99G244")
                ]);
            Add(command);
        }
    }
}