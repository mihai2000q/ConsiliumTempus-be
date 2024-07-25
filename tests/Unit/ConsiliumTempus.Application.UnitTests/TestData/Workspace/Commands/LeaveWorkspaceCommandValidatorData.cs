using ConsiliumTempus.Application.Workspace.Commands.Leave;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class LeaveWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<LeaveWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateLeaveWorkspaceCommand();
            Add(command);

            command = new LeaveWorkspaceCommand(
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<LeaveWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateLeaveWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}