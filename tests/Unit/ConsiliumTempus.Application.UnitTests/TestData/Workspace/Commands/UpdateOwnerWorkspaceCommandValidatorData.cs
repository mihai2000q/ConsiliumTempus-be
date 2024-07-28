using ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class UpdateOwnerWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateOwnerWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateOwnerWorkspaceCommand();
            Add(command);

            command = new UpdateOwnerWorkspaceCommand(
                Guid.NewGuid(),
                Guid.NewGuid());
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateOwnerWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateOwnerWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }

    internal class GetInvalidOwnerIdCommands : TheoryData<UpdateOwnerWorkspaceCommand, string>
    {
        public GetInvalidOwnerIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateOwnerWorkspaceCommand(ownerId: Guid.Empty);
            Add(command, nameof(command.OwnerId));
        }
    }
}