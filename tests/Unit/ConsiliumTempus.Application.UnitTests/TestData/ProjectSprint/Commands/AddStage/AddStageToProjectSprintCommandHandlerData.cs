using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.AddStage;

internal static class AddStageToProjectSprintCommandHandlerData
{
    internal class GetCommands : TheoryData<AddStageToProjectSprintCommand>
    {
        public GetCommands()
        {
            var command = ProjectSprintCommandFactory.CreateAddStageToProjectSprintCommand();
            Add(command);
            
            command = ProjectSprintCommandFactory.CreateAddStageToProjectSprintCommand(onTop: true);
            Add(command);
        }
    }
}