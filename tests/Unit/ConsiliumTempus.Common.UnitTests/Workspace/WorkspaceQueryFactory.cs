using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceQueryFactory
{
    public static GetWorkspaceQuery CreateGetWorkspaceQuery(Guid? id = null)
    {
        return new GetWorkspaceQuery(id ?? Guid.NewGuid());
    }

    public static GetCollectionWorkspaceQuery CreateGetCollectionWorkspaceQuery()
    {
        return new GetCollectionWorkspaceQuery();
    }
}