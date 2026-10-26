using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateCustomField;

internal static class UpdateCustomFieldFromProjectTaskCommandHandlerData
{
    internal class GetCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, ProjectTaskAggregate>
    {
        public GetCommands()
        {
            // Date
            var task = ProjectTaskFactory.Create();
            CustomField customField = CustomFieldFactory.CreateDate();
            task.AddCustomField(customField);
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Date,
                dateCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateCustomFieldCommand(null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateDate();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Date,
                dateCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateCustomFieldCommand(
                    new DateOnly(2022, 10, 10)));
            Add(command, task);

            // Date Time
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateDateTime();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.DateTime,
                dateTimeCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateTimeCustomFieldCommand(null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateDateTime();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.DateTime,
                dateTimeCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateTimeCustomFieldCommand(
                    new DateTime(2022, 10, 10, 10, 55, 30)));
            Add(command, task);

            // Duration
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateDuration();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Duration,
                durationCustomField: new UpdateCustomFieldFromProjectTaskCommand.DurationCustomFieldCommand(null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateDuration();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Duration,
                durationCustomField: new UpdateCustomFieldFromProjectTaskCommand.DurationCustomFieldCommand(
                    new TimeSpan(10, 30, 10)));
            Add(command, task);

            // Multi Select
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateMultiSelect();
            var multiOption = ((MultiSelectCustomField)customField).Setup.Options[0];
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand(
                    multiOption.Id,
                    false));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateMultiSelect();
            multiOption = ((MultiSelectCustomField)customField).Setup.Options[0];
            ((MultiSelectCustomField)customField).AddOption(multiOption);
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand(
                    multiOption.Id,
                    true));
            Add(command, task);

            // Number
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateNumber();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand(null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateNumber();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand(55));
            Add(command, task);

            // People
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreatePeople();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.People,
                peopleCustomField: new UpdateCustomFieldFromProjectTaskCommand.PeopleCustomFieldCommand(null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreatePeople();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.People,
                peopleCustomField: new UpdateCustomFieldFromProjectTaskCommand.PeopleCustomFieldCommand(
                    Guid.NewGuid()));
            Add(command, task);

            // Single Select
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateSingleSelect();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand(
                    null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateSingleSelect();
            task.AddCustomField(customField);
            var option = ((SingleSelectCustomField)customField).Setup.Options[0];
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.SingleSelect,
                singleSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand(
                    option.Id));
            Add(command, task);

            // Text
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateText();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Text,
                textCustomField: new UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand(null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateText();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Text,
                textCustomField: new UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand("Some Text"));
            Add(command, task);

            // Time
            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateTime();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Time,
                timeCustomField: new UpdateCustomFieldFromProjectTaskCommand.TimeCustomFieldCommand(null));
            Add(command, task);

            task = ProjectTaskFactory.Create();
            customField = CustomFieldFactory.CreateTime();
            task.AddCustomField(customField);
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: task.Id.Value,
                customFieldId: customField.Id.Value,
                type: CustomFieldType.Time,
                timeCustomField: new UpdateCustomFieldFromProjectTaskCommand.TimeCustomFieldCommand(
                    new TimeOnly(10, 50)));
            Add(command, task);
        }
    }
}