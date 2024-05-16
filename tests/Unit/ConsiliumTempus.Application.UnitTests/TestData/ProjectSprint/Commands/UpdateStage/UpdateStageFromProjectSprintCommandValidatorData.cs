using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.UpdateStage;

internal static class UpdateStageFromProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateStageFromProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand();
            Add(command);

            command = new UpdateStageFromProjectSprintCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Staging");
            Add(command);
        }
    }

    internal class GetInvalidProjectSprintIdCommands : TheoryData<UpdateStageFromProjectSprintCommand, string>
    {
        public GetInvalidProjectSprintIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand(projectSprintId: Guid.Empty);
            Add(command, nameof(command.ProjectSprintId));
        }
    }

    internal class GetInvalidStageIdCommands : TheoryData<UpdateStageFromProjectSprintCommand, string>
    {
        public GetInvalidStageIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand(stageId: Guid.Empty);
            Add(command, nameof(command.StageId));
        }
    }

    internal class GetInvalidNameCommands : TheoryData<UpdateStageFromProjectSprintCommand, string>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand(name: "");
            Add(command, nameof(command.Name));

            command = ProjectSprintCommandFactory.CreateUpdateStageFromProjectSprintCommand(
                name: new string('*', PropertiesValidation.ProjectStage.NameMaximumLength + 1));
            Add(command, nameof(command.Name));
        }
    }
}