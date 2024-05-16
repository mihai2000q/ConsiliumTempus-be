using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Commands.UpdateOverview;

internal static class UpdateOverviewProjectTaskCommandHandlerData
{
    internal class GetCommands : TheoryData<UpdateOverviewProjectTaskCommand>
    {
        public GetCommands()
        {
            var command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand();
            Add(command);

            command = ProjectTaskCommandFactory.CreateUpdateOverviewProjectTaskCommand(assigneeId: Guid.NewGuid());
            Add(command);
        }
    }
}