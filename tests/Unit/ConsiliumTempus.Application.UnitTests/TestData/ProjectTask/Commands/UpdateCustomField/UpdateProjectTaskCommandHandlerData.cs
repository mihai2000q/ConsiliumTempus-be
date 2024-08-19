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
            // Number
            var task = ProjectTaskFactory.Create();
            CustomField customField = CustomFieldFactory.CreateNumber();
            task.AddCustomField(customField);
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
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
        }
    }
}