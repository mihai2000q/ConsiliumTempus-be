using ConsiliumTempus.Application.Common.Extensions;
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
            // Date
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Date,
                dateCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Date,
                dateCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateCustomFieldCommand(
                    new DateOnly(2002, 01, 01)));
            Add(command);

            // Date Time
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.DateTime,
                dateTimeCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateTimeCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.DateTime,
                dateTimeCustomField: new UpdateCustomFieldFromProjectTaskCommand.DateTimeCustomFieldCommand(
                    new DateTime(2002, 01, 01, 10, 30, 0)));
            Add(command);

            // Duration
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Duration,
                durationCustomField: new UpdateCustomFieldFromProjectTaskCommand.DurationCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Duration,
                durationCustomField: new UpdateCustomFieldFromProjectTaskCommand.DurationCustomFieldCommand(
                    new TimeSpan(10, 30, 55)));
            Add(command);

            // Multi Select
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand(
                    Guid.NewGuid(),
                    true));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand(
                    Guid.NewGuid(),
                    false));
            Add(command);

            // Number
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Number,
                numberCustomField: new UpdateCustomFieldFromProjectTaskCommand.NumberCustomFieldCommand(12));
            Add(command);

            // People
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.People,
                peopleCustomField: new UpdateCustomFieldFromProjectTaskCommand.PeopleCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.People,
                peopleCustomField: new UpdateCustomFieldFromProjectTaskCommand.PeopleCustomFieldCommand(
                    Guid.NewGuid()));
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

            // Time
            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Time,
                timeCustomField: new UpdateCustomFieldFromProjectTaskCommand.TimeCustomFieldCommand(null));
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Time,
                timeCustomField: new UpdateCustomFieldFromProjectTaskCommand.TimeCustomFieldCommand(
                    new TimeOnly(10, 50)));
            Add(command);

            command = new UpdateCustomFieldFromProjectTaskCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                CustomFieldType.SingleSelect,
                null,
                null,
                null,
                null,
                null,
                null,
                new UpdateCustomFieldFromProjectTaskCommand.SingleSelectCustomFieldCommand(null),
                null,
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

    internal class GetInvalidDateCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidDateCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Date,
                dateCustomField: null);
            Add(command, nameof(command.DateCustomField));
        }
    }

    internal class GetInvalidDateTimeCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidDateTimeCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.DateTime,
                dateTimeCustomField: null);
            Add(command, nameof(command.DateTimeCustomField));
        }
    }

    internal class GetInvalidDurationCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidDurationCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Duration,
                durationCustomField: null);
            Add(command, nameof(command.DurationCustomField));
        }
    }

    internal class
        GetInvalidMultiSelectCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidMultiSelectCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: null);
            Add(command, nameof(command.MultiSelectCustomField));

            command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.MultiSelect,
                multiSelectCustomField: new UpdateCustomFieldFromProjectTaskCommand.MultiSelectCustomFieldCommand(
                    Guid.Empty,
                    true));
            Add(command, nameof(command.MultiSelectCustomField)
                .Dot(nameof(command.MultiSelectCustomField.OptionId)));
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

    internal class GetInvalidPeopleCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidPeopleCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.People,
                peopleCustomField: null);
            Add(command, nameof(command.PeopleCustomField));
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

    internal class GetInvalidTimeCustomFieldCommands : TheoryData<UpdateCustomFieldFromProjectTaskCommand, string>
    {
        public GetInvalidTimeCustomFieldCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateCustomFieldFromProjectTaskCommand(
                type: CustomFieldType.Time,
                timeCustomField: null);
            Add(command, nameof(command.TimeCustomField));
        }
    }
}