using ConsiliumTempus.Application.Project.Entities.Stage.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Commands.Create;

internal static class CreateProjectStageCommandHandlerData
{
    internal class GetCommands : TheoryData<CreateProjectStageCommand>
    {
        public GetCommands()
        {
            var command = ProjectStageCommandFactory.CreateCreateProjectStageCommand();
            Add(command);
            
            command = ProjectStageCommandFactory.CreateCreateProjectStageCommand(onTop: true);
            Add(command);
        }
    }
}