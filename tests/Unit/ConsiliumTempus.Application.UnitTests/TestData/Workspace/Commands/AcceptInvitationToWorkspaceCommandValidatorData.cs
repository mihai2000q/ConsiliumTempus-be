using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class AcceptInvitationToWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<AcceptInvitationToWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateAcceptInvitationToWorkspaceCommand();
            Add(command);

            command = new AcceptInvitationToWorkspaceCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<AcceptInvitationToWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateAcceptInvitationToWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidInvitationIdCommands : TheoryData<AcceptInvitationToWorkspaceCommand, string>
    {
        public GetInvalidInvitationIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateAcceptInvitationToWorkspaceCommand(invitationId: Guid.Empty);
            Add(command, nameof(command.InvitationId));
        }
    }
}