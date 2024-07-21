using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceCommandFactory
{
    public static CreateWorkspaceCommand CreateCreateWorkspaceCommand(
        string name = Constants.Workspace.Name)
    {
        return new CreateWorkspaceCommand(
            name);
    }

    public static DeleteWorkspaceCommand CreateDeleteWorkspaceCommand(Guid? id = null)
    {
        return new DeleteWorkspaceCommand(id ?? Guid.NewGuid());
    }

    public static InviteCollaboratorToWorkspaceCommand CreateInviteCollaboratorToWorkspaceCommand(
        Guid? id = null,
        string email = Constants.User.Email)
    {
        return new InviteCollaboratorToWorkspaceCommand(
            id ?? Guid.NewGuid(),
            email);
    }

    public static UpdateWorkspaceCommand CreateUpdateWorkspaceCommand(
        Guid? id = null,
        string name = Constants.Workspace.Name)
    {
        return new UpdateWorkspaceCommand(
            id ?? Guid.NewGuid(),
            name);
    }

    public static UpdateFavoritesWorkspaceCommand CreateUpdateFavoriteWorkspaceCommand(
        Guid? id = null,
        bool isFavorite = false)
    {
        return new UpdateFavoritesWorkspaceCommand(
            id ?? Guid.NewGuid(),
            isFavorite);
    }
    
    public static UpdateOverviewWorkspaceCommand CreateUpdateOverviewWorkspaceCommand(
        Guid? id = null,
        string description = Constants.Workspace.Description)
    {
        return new UpdateOverviewWorkspaceCommand(
            id ?? Guid.NewGuid(),
            description);
    }
}