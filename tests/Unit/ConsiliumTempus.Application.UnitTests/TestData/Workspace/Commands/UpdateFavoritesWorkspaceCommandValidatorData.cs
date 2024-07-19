using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Commands;

internal static class UpdateFavoritesWorkspaceCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateFavoritesWorkspaceCommand>
    {
        public GetValidCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateFavoriteWorkspaceCommand();
            Add(command);

            command = new UpdateFavoritesWorkspaceCommand(
                Guid.NewGuid(),
                true);
            Add(command);
        }
    }
    
    internal class GetInvalidIdCommands : TheoryData<UpdateFavoritesWorkspaceCommand, string>
    {
        public GetInvalidIdCommands()
        {
            var command = WorkspaceCommandFactory.CreateUpdateFavoriteWorkspaceCommand(id: Guid.Empty);
            Add(command, nameof(command.Id));
        }
    }
}