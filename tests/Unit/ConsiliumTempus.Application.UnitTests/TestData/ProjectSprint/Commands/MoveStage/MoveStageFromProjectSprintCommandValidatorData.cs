using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.MoveStage;

internal static class MoveStageFromProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<MoveStageFromProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand();
            Add(command);

            command = new MoveStageFromProjectSprintCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<MoveStageFromProjectSprintCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(projectSprintId: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidStageIdCommands : TheoryData<MoveStageFromProjectSprintCommand, string>
    {
        public GetInvalidStageIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(stageId: Guid.Empty);
            Add(command, nameof(command.StageId));
        }
    }

    internal class GetInvalidOverStageIdCommands : TheoryData<MoveStageFromProjectSprintCommand, string>
    {
        public GetInvalidOverStageIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(overStageId: Guid.Empty);
            Add(command, nameof(command.OverStageId));
        }
    }
}