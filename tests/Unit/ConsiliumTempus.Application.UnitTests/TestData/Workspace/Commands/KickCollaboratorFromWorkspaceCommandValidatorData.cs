using ConsiliumTempus.Application.Workspace.Commands.KickCollaborator;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class KickCollaboratorFromWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<KickCollaboratorFromWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand();
            Add(command);

            command = new KickCollaboratorFromWorkspaceCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<KickCollaboratorFromWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidCollaboratorIdCommands : TheoryData<KickCollaboratorFromWorkspaceCommand, string>
    {
        public GetInvalidCollaboratorIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateKickCollaboratorFromWorkspaceCommand(
                collaboratorId: Guid.Empty);
            Add(command, nameof(command.CollaboratorId));
        }
    }
}