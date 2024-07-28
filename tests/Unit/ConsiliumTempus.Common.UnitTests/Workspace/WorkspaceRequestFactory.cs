using ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.Contracts.Workspace.GetInvitations;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;
using ConsiliumTempus.Api.Contracts.Workspace.Leave;
using ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOverview;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOwner;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceRequestFactory
{
    public static GetWorkspaceRequest CreateGetWorkspaceRequest(Guid? id = null)
    {
        return new GetWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetOverviewWorkspaceRequest CreateGetOverviewWorkspaceRequest(Guid? id = null)
    {
        return new GetOverviewWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionWorkspaceRequest CreateGetCollectionWorkspaceRequest(
        bool isPersonalWorkspaceFirst = false,
        int? pageSize = null,
        int? currentPage = null,
        string[]? orderBy = null,
        string[]? search = null)
    {
        return new GetCollectionWorkspaceRequest
        {
            IsPersonalWorkspaceFirst = isPersonalWorkspaceFirst,
            PageSize = pageSize,
            CurrentPage = currentPage,
            OrderBy = orderBy,
            Search = search
        };
    }

    public static GetCollaboratorsFromWorkspaceRequest CreateGetCollaboratorsFromWorkspaceRequest(
        Guid? id = null,
        string searchValue = "")
    {
        return new GetCollaboratorsFromWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid(),
            SearchValue = searchValue
        };
    }

    public static GetInvitationsWorkspaceRequest CreateGetInvitationsWorkspaceRequest(
        bool? isSender = null,
        Guid? workspaceId = null,
        int? pageSize = null,
        int? currentPage = null)
    {
        return new GetInvitationsWorkspaceRequest
        {
            IsSender = isSender,
            WorkspaceId = workspaceId,
            PageSize = pageSize,
            CurrentPage = currentPage
        };
    }

    public static CreateWorkspaceRequest CreateCreateWorkspaceRequest(
        string name = Constants.Workspace.Name)
    {
        return new CreateWorkspaceRequest(
            name);
    }

    public static InviteCollaboratorToWorkspaceRequest CreateInviteCollaboratorToWorkspaceRequest(
        Guid? id = null,
        string email = Constants.User.Email)
    {
        return new InviteCollaboratorToWorkspaceRequest(
            id ?? Guid.NewGuid(),
            email);
    }

    public static AcceptInvitationToWorkspaceRequest CreateAcceptInvitationToWorkspaceRequest(
        Guid? id = null,
        Guid? invitationId = null)
    {
        return new AcceptInvitationToWorkspaceRequest(
            id ?? Guid.NewGuid(),
            invitationId ?? Guid.NewGuid());
    }

    public static RejectInvitationToWorkspaceRequest CreateRejectInvitationToWorkspaceRequest(
        Guid? id = null,
        Guid? invitationId = null)
    {
        return new RejectInvitationToWorkspaceRequest(
            id ?? Guid.NewGuid(),
            invitationId ?? Guid.NewGuid());
    }

    public static LeaveWorkspaceRequest CreateLeaveWorkspaceRequest(
        Guid? id = null)
    {
        return new LeaveWorkspaceRequest(
            id ?? Guid.NewGuid());
    }

    public static UpdateWorkspaceRequest CreateUpdateWorkspaceRequest(
        Guid? id = null,
        string name = Constants.Workspace.Name)
    {
        return new UpdateWorkspaceRequest(
            id ?? Guid.NewGuid(),
            name);
    }

    public static UpdateFavoritesWorkspaceRequest CreateUpdateFavoritesWorkspaceRequest(
        Guid? id = null,
        bool isFavorite = false)
    {
        return new UpdateFavoritesWorkspaceRequest(
            id ?? Guid.NewGuid(),
            isFavorite);
    }

    public static UpdateOverviewWorkspaceRequest CreateUpdateOverviewWorkspaceRequest(
        Guid? id = null,
        string description = Constants.Workspace.Description)
    {
        return new UpdateOverviewWorkspaceRequest(
            id ?? Guid.NewGuid(),
            description);
    }

    public static UpdateOwnerWorkspaceRequest CreateUpdateOwnerWorkspaceRequest(
        Guid? id = null,
        Guid? ownerId = null)
    {
        return new UpdateOwnerWorkspaceRequest(
            id ?? Guid.NewGuid(),
            ownerId ?? Guid.NewGuid());
    }

    public static DeleteWorkspaceRequest CreateDeleteWorkspaceRequest(Guid? id = null)
    {
        return new DeleteWorkspaceRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
}