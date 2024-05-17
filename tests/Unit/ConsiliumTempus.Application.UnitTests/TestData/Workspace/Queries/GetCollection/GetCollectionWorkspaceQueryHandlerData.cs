using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Workspace;

namespace ConsiliumTempus.Application.UnitTests.TestData.Workspace.Queries.GetCollection;

internal static class GetCollectionWorkspaceQueryHandlerData
{
    internal class GetQueries : TheoryData<GetCollectionWorkspaceQuery>
    {
        public GetQueries()
        {
            var query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery();
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: "name.asc");
            Add(query);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orders: "last_activity.desc, name.asc",
                pageSize: 25,
                currentPage: 1);
            Add(query);
            
            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                isPersonalWorkspaceFirst: true);
            Add(query);
        }
    }
}