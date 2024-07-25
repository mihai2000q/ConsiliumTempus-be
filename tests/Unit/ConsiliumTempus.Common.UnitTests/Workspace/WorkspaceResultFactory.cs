using ConsiliumTempus.Application.Workspace.Commands.AcceptInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.InviteCollaborator;
using ConsiliumTempus.Application.Workspace.Commands.Leave;
using ConsiliumTempus.Application.Workspace.Commands.RejectInvitation;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOverview;
using ConsiliumTempus.Application.Workspace.Commands.UpdateOwner;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Common.UnitTests.User;
using ConsiliumTempus.Common.UnitTests.Workspace.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.Entities;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceResultFactory
{
    public static GetWorkspaceResult CreateGetWorkspaceResult(
        WorkspaceAggregate? workspace = null,
        UserAggregate? user = null)
    {
        return new GetWorkspaceResult(
            workspace ?? WorkspaceFactory.Create(),
            user ?? UserFactory.Create());
    }

    public static GetCollectionWorkspaceResult CreateGetCollectionWorkspaceResult(
        List<WorkspaceAggregate>? workspaces = null,
        int totalCount = 25,
        UserAggregate? currentUser = null)
    {
        return new GetCollectionWorkspaceResult(
            workspaces ?? WorkspaceFactory.CreateList(),
            totalCount,
            currentUser ?? UserFactory.Create());
    }
    
    public static GetCollaboratorsFromWorkspaceResult CreateGetCollaboratorsFromWorkspaceResult(
        List<UserAggregate>? collaborators = null)
    {
        return new GetCollaboratorsFromWorkspaceResult(
            collaborators ?? UserFactory.CreateList());
    }

    public static GetInvitationsWorkspaceResult CreateGetInvitationsWorkspaceResult(
        List<WorkspaceInvitation>? invitations = null,
        int totalCount = 25)
    {
        return new GetInvitationsWorkspaceResult(
            invitations ?? WorkspaceInvitationFactory.CreateList(),
            TotalCount: totalCount);
    }
    
    public static CreateWorkspaceResult CreateCreateWorkspaceResult()
    {
        return new CreateWorkspaceResult();
    }

    public static InviteCollaboratorToWorkspaceResult CreateInviteCollaboratorToWorkspaceResult()
    {
        return new InviteCollaboratorToWorkspaceResult();
    }

    public static AcceptInvitationToWorkspaceResult CreateAcceptInvitationToWorkspaceResult()
    {
        return new AcceptInvitationToWorkspaceResult();
    }

    public static RejectInvitationToWorkspaceResult CreateRejectInvitationToWorkspaceResult()
    {
        return new RejectInvitationToWorkspaceResult();
    }

    public static LeaveWorkspaceResult CreateLeaveWorkspaceResult()
    {
        return new LeaveWorkspaceResult();
    }
    
    public static UpdateWorkspaceResult CreateUpdateWorkspaceResult()
    {
        return new UpdateWorkspaceResult();
    }

    public static UpdateFavoritesWorkspaceResult CreateUpdateFavoritesWorkspaceResult()
    {
        return new UpdateFavoritesWorkspaceResult();
    }
    
    public static UpdateOverviewWorkspaceResult CreateUpdateOverviewWorkspaceResult()
    {
        return new UpdateOverviewWorkspaceResult();
    }

    public static UpdateOwnerWorkspaceResult CreateUpdateOwnerWorkspaceResult()
    {
        return new UpdateOwnerWorkspaceResult();
    }
    
    public static DeleteWorkspaceResult CreateDeleteWorkspaceResult()
    {
        return new DeleteWorkspaceResult();
    }
}