using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Application.Workspace.Queries.GetInvitations;
using ConsiliumTempus.Application.Workspace.Queries.GetOverview;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceQueryFactory
{
    public static GetWorkspaceQuery CreateGetWorkspaceQuery(Guid? id = null)
    {
        return new GetWorkspaceQuery(id ?? Guid.NewGuid());
    }
    
    public static GetCollaboratorsFromWorkspaceQuery CreateGetCollaboratorsFromWorkspaceQuery(
        Guid? id = null,
        string searchValue = "")
    {
        return new GetCollaboratorsFromWorkspaceQuery(
            id ?? Guid.NewGuid(),
            searchValue);
    }

    public static GetCollectionWorkspaceQuery CreateGetCollectionWorkspaceQuery(
        bool isPersonalWorkspaceFirst = false,
        int? pageSize = null,
        int? currentPage = null,
        string[]? orderBy = null,
        string[]? search = null)
    {
        return new GetCollectionWorkspaceQuery(
            isPersonalWorkspaceFirst,
            pageSize,
            currentPage,
            orderBy,
            search);
    }

    public static GetInvitationsWorkspaceQuery CreateGetInvitationsWorkspaceQuery(
        bool? isSender = null,
        Guid? workspaceId = null,
        int? pageSize = null,
        int? currentPage = null)
    {
        return new GetInvitationsWorkspaceQuery(
            isSender,
            workspaceId,
            pageSize,
            currentPage);
    }

    public static GetOverviewWorkspaceQuery CreateGetOverviewWorkspaceQuery(Guid? id = null)
    {
        return new GetOverviewWorkspaceQuery(id ?? Guid.NewGuid());
    }
}