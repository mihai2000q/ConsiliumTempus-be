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
            // Date
            var dateCustomFieldSetup = CustomFieldSetupFactory.CreateDate();
            var command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: dateCustomFieldSetup.Id.Value,
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new UpdateCustomFieldSetupCommand.DateCustomFieldSetupCommand(
                    null));
            Add(command, dateCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: dateCustomFieldSetup.Id.Value,
                type: CustomFieldType.Date,
                dateCustomFieldSetup: new UpdateCustomFieldSetupCommand.DateCustomFieldSetupCommand(
                    new DateOnly(2022, 10, 10)));
            Add(command, dateCustomFieldSetup);
            
            // Date Time
            var dateTimeCustomFieldSetup = CustomFieldSetupFactory.CreateDateTime();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: dateTimeCustomFieldSetup.Id.Value,
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new UpdateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand(
                    null));
            Add(command, dateTimeCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: dateTimeCustomFieldSetup.Id.Value,
                type: CustomFieldType.DateTime,
                dateTimeCustomFieldSetup: new UpdateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand(
                    new DateTime(2022, 10, 10, 10, 50, 30)));
            Add(command, dateTimeCustomFieldSetup);
            
            // Duration
            var durationCustomFieldSetup = CustomFieldSetupFactory.CreateDuration();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: durationCustomFieldSetup.Id.Value,
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new UpdateCustomFieldSetupCommand.DurationCustomFieldSetupCommand(
                    null));
            Add(command, durationCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: durationCustomFieldSetup.Id.Value,
                type: CustomFieldType.Duration,
                durationCustomFieldSetup: new UpdateCustomFieldSetupCommand.DurationCustomFieldSetupCommand(
                    new TimeSpan(10, 12, 50, 43)));
            Add(command, durationCustomFieldSetup);
            
            // Multi Select
            var multiSelectCustomFieldSetup = CustomFieldSetupFactory.CreateMultiSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: multiSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    null,
                    null,
                    null,
                    null));
            Add(command, multiSelectCustomFieldSetup);

            // Single Select - Options
            multiSelectCustomFieldSetup = CustomFieldSetupFactory.CreateMultiSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: multiSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#FF2233",
                        "High"),
                    null,
                    null));
            Add(command, multiSelectCustomFieldSetup);

            multiSelectCustomFieldSetup = CustomFieldSetupFactory.CreateMultiSelect();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: multiSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
                    new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand(
                        "#FF2233",
                        "High"),
                    multiSelectCustomFieldSetup.Options[1].Id,
                    null));
            Add(command, multiSelectCustomFieldSetup);

            multiSelectCustomFieldSetup = CustomFieldSetupFactory.CreateMultiSelect(
                options:
                [
                    MultiSelectOptionFactory.Create(),
                    MultiSelectOptionFactory.Create(customOrderPosition: 1),
                    MultiSelectOptionFactory.Create(customOrderPosition: 2),
                    MultiSelectOptionFactory.Create(customOrderPosition: 3),
                    MultiSelectOptionFactory.Create(customOrderPosition: 4),
                ]);
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: multiSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    multiSelectCustomFieldSetup.Options[1].Id,
                    multiSelectCustomFieldSetup.Options[3].Id));
            Add(command, multiSelectCustomFieldSetup);

            multiSelectCustomFieldSetup = CustomFieldSetupFactory.CreateMultiSelect(
                options:
                [
                    MultiSelectOptionFactory.Create(),
                    MultiSelectOptionFactory.Create(customOrderPosition: 1),
                    MultiSelectOptionFactory.Create(customOrderPosition: 2),
                    MultiSelectOptionFactory.Create(customOrderPosition: 3),
                    MultiSelectOptionFactory.Create(customOrderPosition: 4),
                ]);
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: multiSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
                    null,
                    multiSelectCustomFieldSetup.Options[3].Id,
                    multiSelectCustomFieldSetup.Options[1].Id));
            Add(command, multiSelectCustomFieldSetup);

            multiSelectCustomFieldSetup = CustomFieldSetupFactory.CreateMultiSelect(
                options:
                [
                    MultiSelectOptionFactory.Create(),
                    MultiSelectOptionFactory.Create(customOrderPosition: 1),
                    MultiSelectOptionFactory.Create(customOrderPosition: 2),
                    MultiSelectOptionFactory.Create(customOrderPosition: 3),
                    MultiSelectOptionFactory.Create(customOrderPosition: 4),
                ]);
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: multiSelectCustomFieldSetup.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomFieldSetup: new UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand(
                    UpdateCustomFieldSetupCommand.OptionOperation.Remove,
                    null,
                    multiSelectCustomFieldSetup.Options[2].Id,
                    null));
            Add(command, multiSelectCustomFieldSetup);

            // Number
            var numberCustomFieldSetup = CustomFieldSetupFactory.CreateNumber();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
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
            
            // People
            var peopleCustomFieldSetup = CustomFieldSetupFactory.CreatePeople();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: peopleCustomFieldSetup.Id.Value,
                type: CustomFieldType.People);
            Add(command, peopleCustomFieldSetup);

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
                    UpdateCustomFieldSetupCommand.OptionOperation.Add,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Remove,
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
            
            // Time
            var timeCustomFieldSetup = CustomFieldSetupFactory.CreateTime();
            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: timeCustomFieldSetup.Id.Value,
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new UpdateCustomFieldSetupCommand.TimeCustomFieldSetupCommand(
                    null));
            Add(command, timeCustomFieldSetup);

            command = CustomFieldSetupCommandFactory.CreateUpdateCustomFieldSetupCommand(
                id: timeCustomFieldSetup.Id.Value,
                type: CustomFieldType.Time,
                timeCustomFieldSetup: new UpdateCustomFieldSetupCommand.TimeCustomFieldSetupCommand(
                    new TimeOnly(5, 10, 10)));
            Add(command, timeCustomFieldSetup);
        }
    }

    internal class
        GetSingleSelectCommands : TheoryData<UpdateCustomFieldSetupCommand, SingleSelectCustomFieldSetupAggregate>
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Update,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Move,
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
                    UpdateCustomFieldSetupCommand.OptionOperation.Remove,
                    null,
                    Guid.NewGuid(),
                    null));
            Add(command, singleSelectCustomFieldSetup);
        }
    }
}