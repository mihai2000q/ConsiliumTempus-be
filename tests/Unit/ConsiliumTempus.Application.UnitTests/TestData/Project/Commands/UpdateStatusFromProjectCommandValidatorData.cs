using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class UpdateStatusFromProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateStatusFromProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand();
            Add(command);

            command = new UpdateStatusFromProjectCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Status Update - 30 May",
                "completed",
                "This is the description of the new status update");
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateStatusFromProjectCommand, string, short>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id), 1);
        }
    }

    internal class GetInvalidStatusIdCommands : TheoryData<UpdateStatusFromProjectCommand, string, short>
    {
        public GetInvalidStatusIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(statusId: Guid.Empty);
            Add(command, nameof(command.StatusId), 1);
        }
    }

    internal class GetInvalidTitleCommands : TheoryData<UpdateStatusFromProjectCommand, string, short>
    {
        public GetInvalidTitleCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(title: "");
            Add(command, nameof(command.Title), 1);

            command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(
                title: new string('a', PropertiesValidation.ProjectStatus.TitleMaximumLength + 1));
            Add(command, nameof(command.Title), 1);
        }
    }

    internal class GetInvalidStatusCommands : TheoryData<UpdateStatusFromProjectCommand, string, short>
    {
        public GetInvalidStatusCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommandWithStatus("");
            Add(command, nameof(command.Status), 2);

            command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommandWithStatus("something");
            Add(command, nameof(command.Status), 1);

            command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommandWithStatus("on track");
            Add(command, nameof(command.Status), 1);

            command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommandWithStatus("On Track");
            Add(command, nameof(command.Status), 1);
        }
    }

    internal class GetInvalidDescriptionCommands : TheoryData<UpdateStatusFromProjectCommand, string, short>
    {
        public GetInvalidDescriptionCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateStatusFromProjectCommand(description: "");
            Add(command, nameof(command.Description), 1);
        }
    }
}