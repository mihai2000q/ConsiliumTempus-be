using ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;
using ConsiliumTempus.Common.UnitTests.Common.Entities;
using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;

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
                    null,
                    null,
                    null,
                    null,
                    null));
            Add(command, singleSelectCustomFieldSetup);

            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    null,
                    null,
                    null,
                    null));
            Add(command, singleSelectCustomFieldSetup);

            // Single Select - Options
            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[1].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#FF2233",
                        "High"),
                    null,
                    null));
            Add(command, singleSelectCustomFieldSetup);

            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Update,
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#FF2233",
                        "High"),
                    singleSelectCustomFieldSetup.Options[1].Id,
                    null));
            Add(command, singleSelectCustomFieldSetup);

            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect(
                options: 
                [
                    SingleSelectOptionFactory.Create(), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 1), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 2), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 3), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 4), 
                ]);
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move,
                    null,
                    singleSelectCustomFieldSetup.Options[1].Id,
                    singleSelectCustomFieldSetup.Options[3].Id));
            Add(command, singleSelectCustomFieldSetup);
            
            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect(
                options: 
                [
                    SingleSelectOptionFactory.Create(), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 1), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 2), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 3), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 4), 
                ]);
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move,
                    null,
                    singleSelectCustomFieldSetup.Options[3].Id,
                    singleSelectCustomFieldSetup.Options[1].Id));
            Add(command, singleSelectCustomFieldSetup);

            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect(
                options: 
                [
                    SingleSelectOptionFactory.Create(), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 1), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 2), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 3), 
                    SingleSelectOptionFactory.Create(customOrderPosition: 4), 
                ]);
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Remove,
                    null,
                    singleSelectCustomFieldSetup.Options[2].Id,
                    null));
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
    
    internal class GetSingleSelectCommands : TheoryData<UpdateCustomFieldSetupCommand, SingleSelectCustomFieldSetupAggregate>
    {
        public GetSingleSelectCommands()
        {
            // Single Select
            var singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    null));
            Add(command, singleSelectCustomFieldSetup);

            // Single Select - Options
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Update,
                    new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand(
                        "#FF2233",
                        "High"),
                    Guid.NewGuid(),
                    null));
            Add(command, singleSelectCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move,
                    null,
                    Guid.NewGuid(),
                    singleSelectCustomFieldSetup.Options[1].Id));
            Add(command, singleSelectCustomFieldSetup);
            
            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Move,
                    null,
                    singleSelectCustomFieldSetup.Options[1].Id,
                    Guid.NewGuid()));
            Add(command, singleSelectCustomFieldSetup);

            singleSelectCustomFieldSetup = CustomFieldSetupFactory.CreateSingleSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: singleSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand(
                    singleSelectCustomFieldSetup.Options[0].Id,
                    UpdateCustomFieldSetupCommand.SingleSelectOptionOperation.Remove,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command, singleSelectCustomFieldSetup);
        }
    }
}