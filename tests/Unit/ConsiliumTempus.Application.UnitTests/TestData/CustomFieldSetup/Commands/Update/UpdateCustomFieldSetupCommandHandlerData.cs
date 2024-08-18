using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Update;

internal static class UpdateCustomFieldSetupCommandHandlerData
{
    internal class GetCommands : TheoryData<UpdateCustomFieldSetupCommand, CustomFieldSetupAggregate>
    {
        public GetCommands()
        {
            // Number
            var numberCustomFieldSetup = CustomFieldSetupFactory.CreateNumber();
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: numberCustomFieldSetup.Id.Value,
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "USD",
                        2,
                        true),
                    null));
            Add(command, numberCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: numberCustomFieldSetup.Id.Value,
                type: CustomFieldType.Number,
                numberCustomFieldSetup: new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand(
                    new UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand.NumberSettingsCommand(
                        "EUR",
                        3,
                        false),
                    12));
            Add(command, numberCustomFieldSetup);

            // Single Select
            var singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    null));
            Add(command, singleSelectCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    Guid.NewGuid()));
            Add(command, singleSelectCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[1].Id));
            Add(command, singleSelectCustomFieldSetup);

            // Text
            var textCustomFieldSetup = CustomFieldSetupFactory.CreateText();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: textCustomFieldSetup.Id.Value,
                type: CustomFieldType.Text,
                textCustomFieldSetup: new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand(null));
            Add(command, textCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: textCustomFieldSetup.Id.Value,
                type: CustomFieldType.Text,
                textCustomFieldSetup: new UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand("Default Text"));
            Add(command, textCustomFieldSetup);
        }
    }
}