using ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class UpdateCollaboratorFromWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateCollaboratorFromWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand();
            Add(command);

            command = new UpdateCollaboratorFromWorkspaceCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                nameof(WorkspaceRole.Admin).ToLower());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateCollaboratorFromWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidCollaboratorIdCommands : TheoryData<UpdateCollaboratorFromWorkspaceCommand, string>
    {
        public GetInvalidCollaboratorIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand(
                collaboratorId: Guid.Empty);
            Add(command, nameof(command.CollaboratorId));
        }
    }

    internal class GetInvalidWorkspaceRoleCommands : TheoryData<UpdateCollaboratorFromWorkspaceCommand, string>
    {
        public GetInvalidWorkspaceRoleCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand(
                workspaceRole: "");
            Add(command, nameof(command.WorkspaceRole));

            command = WorkspaceCommandFactory.CreateUpdateCollaboratorFromWorkspaceCommand(
                workspaceRole: "not-a-role");
            Add(command, nameof(command.WorkspaceRole));
        }
    }
}