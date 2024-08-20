using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class AddStatusToProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<AddStatusToProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateAddStatusToProjectCommand();
            Add(command);

            command = new AddStatusToProjectCommand(
                Guid.NewGuid(),
                "New Status",
                ProjectStatusType.OnTrack,
                "Some new description");
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<AddStatusToProjectCommand, string, short>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateAddStatusToProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id), 1);
        }
    }

    internal class GetInvalidTitleCommands : TheoryData<AddStatusToProjectCommand, string, short>
    {
        public GetInvalidTitleCommands()
        {
            var command = ProjectCommandFactory.CreateAddStatusToProjectCommand(title: "");
            Add(command, nameof(command.Title), 1);

            command = ProjectCommandFactory.CreateAddStatusToProjectCommand(
                title: new string('a', PropertiesValidation.ProjectStatus.TitleMaximumLength + 1));
            Add(command, nameof(command.Title), 1);
        }
    }

    internal class GetInvalidDescriptionCommands : TheoryData<AddStatusToProjectCommand, string, short>
    {
        public GetInvalidDescriptionCommands()
        {
            var command = ProjectCommandFactory.CreateAddStatusToProjectCommand(description: "");
            Add(command, nameof(command.Description), 1);
        }
    }
}