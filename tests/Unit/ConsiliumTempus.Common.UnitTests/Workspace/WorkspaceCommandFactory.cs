using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.Leave;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Common.Entities;

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

    public static AcceptInvitationToWorkspaceCommand CreateAcceptInvitationToWorkspaceCommand(
        Guid? id = null,
        Guid? invitationId = null)
    {
        return new AcceptInvitationToWorkspaceCommand(
            id ?? Guid.NewGuid(),
            invitationId ?? Guid.NewGuid());
    }

    public static RejectInvitationToWorkspaceCommand CreateRejectInvitationToWorkspaceCommand(
        Guid? id = null,
        Guid? invitationId = null)
    {
        return new RejectInvitationToWorkspaceCommand(
            id ?? Guid.NewGuid(),
            invitationId ?? Guid.NewGuid());
    }

    public static LeaveWorkspaceCommand CreateLeaveWorkspaceCommand(
        Guid? id = null)
    {
        return new LeaveWorkspaceCommand(
            id ?? Guid.NewGuid());
    }

    public static UpdateWorkspaceCommand CreateUpdateWorkspaceCommand(
        Guid? id = null,
        string name = Constants.Workspace.Name)
    {
        return new UpdateWorkspaceCommand(
            id ?? Guid.NewGuid(),
            name);
    }

    public static UpdateCollaboratorFromWorkspaceCommand CreateUpdateCollaboratorFromWorkspaceCommand(
        Guid? id = null,
        Guid? collaboratorId = null,
        string? workspaceRole = null)
    {
        return new UpdateCollaboratorFromWorkspaceCommand(
            id ?? Guid.NewGuid(),
            collaboratorId ?? Guid.NewGuid(),
            workspaceRole ?? nameof(WorkspaceRole.Admin));
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

    public static UpdateOwnerWorkspaceCommand CreateUpdateOwnerWorkspaceCommand(
        Guid? id = null,
        Guid? ownerId = null)
    {
        return new UpdateOwnerWorkspaceCommand(
            id ?? Guid.NewGuid(),
            ownerId ?? Guid.NewGuid());
    }
}