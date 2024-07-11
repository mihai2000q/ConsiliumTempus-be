using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;

internal static class MoveProjectTaskCommandHandlerData
{
    internal class GetCommands : TheoryData<MoveProjectTaskCommand, ProjectTaskAggregate, List<ProjectStage>>
    {
        public GetCommands()
        {
            var task = ProjectTaskFactory.Create();
            var stages = ProjectStageFactory.CreateList();
            
            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand();
            Add(command, task, stages);
        }
    }
}