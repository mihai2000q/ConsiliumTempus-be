using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceQueryFactory
{
    public static GetWorkspaceQuery CreateGetWorkspaceQuery(Guid? id = null)
    {
        return new GetWorkspaceQuery(id ?? Guid.NewGuid());
    }

    public static GetCollectionWorkspaceQuery CreateGetCollectionWorkspaceQuery(
        bool isPersonalWorkspaceFirst = false,
        int? pageSize = null,
        int? currentPage = null,
        string? orders = null,
        string? name = null)
    {
        return new GetCollectionWorkspaceQuery(
            isPersonalWorkspaceFirst,
            pageSize,
            currentPage,
            orders,
            name);
    }
}