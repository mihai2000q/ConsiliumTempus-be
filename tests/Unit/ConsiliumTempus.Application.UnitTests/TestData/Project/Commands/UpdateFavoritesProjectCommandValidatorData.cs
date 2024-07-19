using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Commands;

internal static class UpdateFavoritesProjectCommandValidatorData
{
    internal class GetValidCommands : TheoryData<UpdateFavoritesProjectCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateFavoritesProjectCommand();
            Add(command);

            command = new UpdateFavoritesProjectCommand(
                Guid.NewGuid(),
                true);
            Add(command);
        }
    }

    internal class GetInvalidIdCommands : TheoryData<UpdateFavoritesProjectCommand, string, short>
    {
        public GetInvalidIdCommands()
        {
            var command = ProjectCommandFactory.CreateUpdateFavoritesProjectCommand(id: Guid.Empty);
            Add(command, nameof(command.Id), 1);
        }
    }
}