using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class UpdateOverviewWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateOverviewWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateOverviewWorkspaceCommand();
            Add(command);

            command = new UpdateOverviewWorkspaceCommand(
                Guid.NewGuid(),
                "This is the new description of the Team");
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<UpdateOverviewWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateOverviewWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}