using ConsiliumTempus.Application.ProjectTask.Commands.UpdateCustomField;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateCustomField;

internal static class UpdateCustomFieldFromProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand>
    {
        public GetValidCommands()
        {
            // Number
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand(12));
            Add(command);

            // Single Select
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand(
                    null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand(
                    Guid.NewGuid()));
            Add(command);

            // Text
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Text,
                textCustomField: new UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Text,
                textCustomField: new UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand("New Text"));
            Add(command);

            command = new UpdateCustomFieldFromProjectTaskCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                CustomFieldType.SingleSelect,
                null,
                new UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand(null),
                null);
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                id: Guid.Empty,
                textCustomField: new UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand(null));
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidCustomFieldIdCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidCustomFieldIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                customFieldId: Guid.Empty,
                textCustomField: new UpdateCustomFieldFromProjectTaskCommand.TextCustomFieldCommand(null));
            Add(command, nameof(command.CustomFieldId));
        }
    }

    internal class GetInvalidNumberCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidNumberCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Number,
                numberCustomField: null);
            Add(command, nameof(command.NumberCustomField));
        }
    }

    internal class
        GetInvalidSingleSelectCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidSingleSelectCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.SingleSelect,
                singleSelectCustomField: null);
            Add(command, nameof(command.SingleSelectCustomField));
        }
    }

    internal class GetInvalidTextCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidTextCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Text,
                textCustomField: null);
            Add(command, nameof(command.TextCustomField));
        }
    }
}