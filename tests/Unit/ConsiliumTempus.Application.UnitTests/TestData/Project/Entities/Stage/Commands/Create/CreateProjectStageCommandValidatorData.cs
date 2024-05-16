using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands.Create;

internal static class CreateProjectStageCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateProjectStageCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectStageCommandFactory.CreateCreateProjectStageCommand();
            Add(command);

            command = new CreateProjectStageCommand(
                Guid.NewGuid(),
                "In Progress",
                false);
            Add(command);
        }
    }

    internal class GetInvalidProjectSprintIdCommands : TheoryData<CreateProjectStageCommand, string>
    {
        public GetInvalidProjectSprintIdCommands()
        {
            var command = ProjectStageCommandFactory.CreateCreateProjectStageCommand(
                projectSprintId: Guid.Empty);
            Add(command, nameof(command.ProjectSprintId));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateProjectStageCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectStageCommandFactory.CreateCreateProjectStageCommand(
                name: "");
            Add(command, nameof(command.Name));

            command = ProjectStageCommandFactory.CreateCreateProjectStageCommand(
                name: new string('*', PropertiesValidation.ProjectStage.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}