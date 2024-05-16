using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.Update;

internal static class UpdateProjectTaskCommandHandlerData
{
    internal class GetCommands : TheoryData<UpdateProjectTaskCommand>
    {
        public GetCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand();
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateProjectTaskCommand(assigneeId: Guid.NewGuid());
            Add(command);
        }
    }
}