using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class UpdateOverviewProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateOverviewProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateOverviewProjectCommand();
            Add(command);
            
            command = new UpdateOverviewProjectCommand(
                Guid.NewGuid(),
                "New Description");
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<UpdateOverviewProjectCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateOverviewProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}