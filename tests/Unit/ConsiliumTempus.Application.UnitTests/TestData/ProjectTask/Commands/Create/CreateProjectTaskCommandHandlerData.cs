using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Create;

internal static class CreateProjectTaskCommandHandlerData
{
    internal class GetCommands : TheoryData<CreateProjectTaskCommand>
    {
        public GetCommands()
        {
            var command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand();
            Add(command);
            
            command = ProjectTaskCommandFactory.CreateCreateProjectTaskCommand(onTop: true);
            Add(command);
        }
    }
}