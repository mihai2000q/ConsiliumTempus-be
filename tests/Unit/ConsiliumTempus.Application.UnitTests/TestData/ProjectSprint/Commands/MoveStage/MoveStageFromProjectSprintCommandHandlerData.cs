using ConsiliumTempus.Application.ProjectSprint.Commands.MoveStage;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.MoveStage;

internal static class MoveStageFromProjectSprintCommandHandlerData
{
    internal class GetCommands
        : TheoryData<MoveStageFromProjectSprintCommand, ProjectSprintAggregate, ProjectStage, ProjectStage>
    {
        public GetCommands()
        {
            // Use Case 1: from lower to upper position
            var sprint = ProjectSprintFactory.Create(stagesCount: 5);

            var stage = sprint.Stages[0];
            var overStage = sprint.Stages[4];

            var command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(
                stageId: stage.Id.Value,
                overStageId: overStage.Id.Value);

            Add(command, sprint, stage, overStage);
            
            // Use Case 2
            sprint = ProjectSprintFactory.Create(stagesCount: 10);

            stage = sprint.Stages[2];
            overStage = sprint.Stages[7];

            command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(
                stageId: stage.Id.Value,
                overStageId: overStage.Id.Value);

            Add(command, sprint, stage, overStage);
            
            // Use Case 2: from upper to lower position
            sprint = ProjectSprintFactory.Create(stagesCount: 5);

            stage = sprint.Stages[4];
            overStage = sprint.Stages[0];

            command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(
                stageId: stage.Id.Value,
                overStageId: overStage.Id.Value);

            Add(command, sprint, stage, overStage);
            
            // Use Case 3
            sprint = ProjectSprintFactory.Create(stagesCount: 5);

            stage = sprint.Stages[3];
            overStage = sprint.Stages[2];

            command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(
                stageId: stage.Id.Value,
                overStageId: overStage.Id.Value);

            Add(command, sprint, stage, overStage);
            
            // Use Case 4
            sprint = ProjectSprintFactory.Create(stagesCount: 10);

            stage = sprint.Stages[7];
            overStage = sprint.Stages[2];

            command = ProjectSprintCommandFactory.CreateMoveStageFromProjectSprintCommand(
                stageId: stage.Id.Value,
                overStageId: overStage.Id.Value);

            Add(command, sprint, stage, overStage);
        }
    }
}