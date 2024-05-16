using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.RemoveStage;

internal static class RemoveStageFromProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<RemoveStageFromProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateRemoveStageFromProjectSprintCommand();
            Add(command);

            command = new RemoveStageFromProjectSprintCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidProjectSprintIdCommands : TheoryData<RemoveStageFromProjectSprintCommand, string>
    {
        public GetInvalidProjectSprintIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateRemoveStageFromProjectSprintCommand(projectSprintId: Guid.Empty);
            Add(command, nameof(command.ProjectSprintId));
        }
    }

    internal class GetInvalidStageIdCommands : TheoryData<RemoveStageFromProjectSprintCommand, string>
    {
        public GetInvalidStageIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateRemoveStageFromProjectSprintCommand(stageId: Guid.Empty);
            Add(command, nameof(command.StageId));
        }
    }
}