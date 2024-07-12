using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;

internal static class MoveProjectTaskCommandHandlerData
{
    internal class GetMovingToAnotherStageCommands 
        : TheoryData<MoveProjectTaskCommand, ProjectTaskAggregate, List<ProjectStage>>
    {
        public GetMovingToAnotherStageCommands()
        {
            // Use Case 1:
            // (normally not possible for the client to send a request of this type when the over stage is not empty)
            var sprint = ProjectSprintFactory.Create(stagesCount: 0);
            var stages = ProjectStageFactory.CreateListWithTasks(
                sprint: sprint,
                stagesCount: 4,
                tasksCount: 10);

            var currentStage = stages[0];
            var task = currentStage.Tasks[4];
            var overStage = stages[1];

            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                id: task.Id.Value,
                overId: overStage.Id.Value);
            Add(command, task, stages);
            
            // Use Case 2: (more likely to happen)
            sprint = ProjectSprintFactory.Create(stagesCount: 0);
            stages = ProjectStageFactory.CreateListWithTasks(
                sprint: sprint,
                stagesCount: 2,
                tasksCount: 10);
            sprint.AddStages(stages);

            currentStage = stages[1];
            task = currentStage.Tasks[4];
            overStage = ProjectStageFactory.Create(sprint: sprint, customOrderPosition: stages.Count);
            sprint.AddStage(overStage);

            command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                id: task.Id.Value,
                overId: overStage.Id.Value);
            Add(command, task, sprint.Stages.ToList());
        }
    }

    internal class GetMovingWithinStageCommands 
        : TheoryData<MoveProjectTaskCommand, ProjectTaskAggregate, List<ProjectStage>, int>
    {
        public GetMovingWithinStageCommands()
        {
            var task = ProjectTaskFactory.CreateWithTasks(tasksCount: 6);
            var overTask = task.Stage.Tasks[5];
            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                id: task.Id.Value,
                overId: overTask.Id.Value);
            Add(command, task, [task.Stage], overTask.CustomOrderPosition.Value);
        }
    }

    internal class GetMovingOverTaskInAnotherStageCommands
        : TheoryData<MoveProjectTaskCommand, ProjectTaskAggregate, List<ProjectStage>, int>
    {
        public GetMovingOverTaskInAnotherStageCommands()
        {
            var stages = ProjectStageFactory.CreateListWithTasks(
                sprint: ProjectSprintFactory.Create(),
                stagesCount: 4, 
                tasksCount: 10);

            var currentStage = stages[0];
            var task = currentStage.Tasks[1];
            var overStage = stages[1];
            var overTask = overStage.Tasks[4];

            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                id: task.Id.Value,
                overId: overTask.Id.Value);
            Add(command, task, stages, overTask.CustomOrderPosition.Value + 1);
        }
    }
}