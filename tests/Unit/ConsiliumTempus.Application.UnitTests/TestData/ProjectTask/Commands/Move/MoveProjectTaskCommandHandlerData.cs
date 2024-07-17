using ConsiliumTempus.Application.ProjectTask.Commands.Move;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Common.UnitTests.ProjectSprint.Entities;
using ConsiliumTempus.Common.UnitTests.ProjectTask;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Move;

internal static class MoveProjectTaskCommandHandlerData
{
    internal class GetMovingToAnotherStageCommands 
        : TheoryData<MoveProjectTaskCommand, ProjectSprintAggregate, ProjectTaskAggregate>
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
            sprint.AddStages(stages);

            var currentStage = stages[0];
            var task = currentStage.Tasks[4];
            var overStage = stages[1];

            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                sprintId: sprint.Id.Value,
                id: task.Id.Value,
                overId: overStage.Id.Value);
            Add(command, sprint, task);
            
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
                sprintId: sprint.Id.Value,
                id: task.Id.Value,
                overId: overStage.Id.Value);
            Add(command, sprint, task);
        }
    }

    internal class GetMovingWithinStageCommands 
        : TheoryData<MoveProjectTaskCommand, ProjectSprintAggregate, ProjectTaskAggregate, int>
    {
        public GetMovingWithinStageCommands()
        {
            // Use Case 1: Upper position
            AddUseCase(10, 5, 7);
            AddUseCase(10, 0, 5);
            AddUseCase(3, 0, 1);
            AddUseCase(3, 0, 1);
            AddUseCase(2, 0, 1);

            // Lower position
            AddUseCase(10, 5, 1);
            AddUseCase(3, 2, 0);
            AddUseCase(3, 1, 0);
            AddUseCase(2, 1, 0);
        }

        private void AddUseCase(
            int tasksCount,
            int taskIndex,
            int overTaskIndex)
        {
            var sprint = ProjectSprintFactory.Create(stagesCount: 0);
            var task = ProjectTaskFactory.CreateWithTasks(sprint: sprint, tasksCount: tasksCount);
            sprint.AddStage(task.Stage);
            task = task.Stage.Tasks[taskIndex];
            var overTask = task.Stage.Tasks[overTaskIndex];
            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                sprintId: sprint.Id.Value,
                id: task.Id.Value,
                overId: overTask.Id.Value);
            Add(command, sprint, task, overTask.CustomOrderPosition.Value);
        }
    }

    internal class GetMovingOverTaskToAnotherStageCommands
        : TheoryData<MoveProjectTaskCommand, ProjectSprintAggregate, ProjectTaskAggregate, int>
    {
        public GetMovingOverTaskToAnotherStageCommands()
        {
            // upper position
            var sprint = ProjectSprintFactory.Create(stagesCount: 0);
            var stages = ProjectStageFactory.CreateListWithTasks(
                sprint: sprint,
                stagesCount: 4, 
                tasksCount: 10);
            sprint.AddStages(stages);

            var currentStage = stages[0];
            var task = currentStage.Tasks[1];
            var overStage = stages[1];
            var overTask = overStage.Tasks[4];

            var command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                sprintId: sprint.Id.Value,
                id: task.Id.Value,
                overId: overTask.Id.Value);
            Add(command, sprint, task, overTask.CustomOrderPosition.Value);
            
            // lower position
            sprint = ProjectSprintFactory.Create(stagesCount: 0);
            stages = ProjectStageFactory.CreateListWithTasks(
                sprint: sprint,
                stagesCount: 4, 
                tasksCount: 10);
            sprint.AddStages(stages);

            currentStage = stages[0];
            task = currentStage.Tasks[8];
            overStage = stages[1];
            overTask = overStage.Tasks[2];

            command = ProjectTaskCommandFactory.CreateMoveProjectTaskCommand(
                sprintId: sprint.Id.Value,
                id: task.Id.Value,
                overId: overTask.Id.Value);
            Add(command, sprint, task, overTask.CustomOrderPosition.Value);
        }
    }
}