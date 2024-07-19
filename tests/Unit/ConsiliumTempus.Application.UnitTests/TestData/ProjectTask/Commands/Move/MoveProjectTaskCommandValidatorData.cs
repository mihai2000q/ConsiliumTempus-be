using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;

internal static class MoveProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<MoveProjectTaskCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand();
            Add(command);

            command = new MoveProjectTaskCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<MoveProjectTaskCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidOverIdCommands : TheoryData<MoveProjectTaskCommand, string>
    {
        public GetInvalidOverIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(overId: Guid.Empty);
            Add(command, nameof(command.OverId));
        }
    }
}