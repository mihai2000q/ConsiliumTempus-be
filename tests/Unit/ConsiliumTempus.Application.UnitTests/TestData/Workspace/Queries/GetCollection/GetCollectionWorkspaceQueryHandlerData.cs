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
                orderBy: ["name.asc"],
                search: ["name ct Something interesting"]);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy: ["last_activity.desc", "name.asc"],
                pageSize: 25,
                currentPage: 1);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                search:
                [
                    "name sw Something",
                    "is_personal eq false",
                    "last_activity lte 2024-01-15",
                    "updated_date_time gt 2024-06-30"
                ]);
            Add(query);

            query = WorkspaceQueryFactory.CreateGetCollectionWorkspaceQuery(
                orderBy:
                [
                    "name.desc",
                    "last_activity.asc",
                    "created_date_time.desc",
                    "updated_date_time.asc"
                ]);
            Add(query);
        }
    }
}