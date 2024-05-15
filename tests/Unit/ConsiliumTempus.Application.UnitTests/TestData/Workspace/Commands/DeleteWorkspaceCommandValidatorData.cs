using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class DeleteWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<DeleteWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateDeleteWorkspaceCommand();
            Add(command);

            command = new DeleteWorkspaceCommand(Guid.NewGuid());
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<DeleteWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateDeleteWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}