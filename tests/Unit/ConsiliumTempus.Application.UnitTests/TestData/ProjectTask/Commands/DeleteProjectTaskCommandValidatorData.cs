using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands;

internal static class DeleteProjectTaskCommandValidatorData
{
    internal class GetValidCommands : TheoryData<DeleteProjectTaskCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand();
            Add(command);
            
            command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<DeleteProjectTaskCommand, string, int>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand(id: Guid.Empty);
            Add(command, nameof(command.Id), 1);
        }
    }

    internal class GetInvalidProjectStageIdCommands : TheoryData<DeleteProjectTaskCommand, string, int>
    {
        public GetInvalidProjectStageIdCommands()
        {
            var command = ProjectTaskCommandFactory.CreateDeleteProjectTaskCommand(projectStageId: Guid.Empty);
            Add(command, nameof(command.StageId), 1);
        }
    }
}