using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class RejectInvitationToWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<RejectInvitationToWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand();
            Add(command);

            command = new RejectInvitationToWorkspaceCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<RejectInvitationToWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidInvitationIdCommands : TheoryData<RejectInvitationToWorkspaceCommand, string>
    {
        public GetInvalidInvitationIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateRejectInvitationToWorkspaceCommand(invitationId: Guid.Empty);
            Add(command, nameof(command.InvitationId));
        }
    }
}