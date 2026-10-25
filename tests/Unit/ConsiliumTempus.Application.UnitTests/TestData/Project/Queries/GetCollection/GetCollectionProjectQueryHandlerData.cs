using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetCollection;

internal static class GetCollectionProjectQueryHandlerData
{
    internal class GetQueries : TheoryData<GetCollectionProjectQuery>
    {
        public GetQueries()
        {
            var query = ProjectQueryFactory.CreateGetCollectionProjectQuery();
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy: ["last_activity.asc", "name.asc"]);
            Add(query);
            
            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                pageSize: 12,
                currentPage: 1,
                orderBy: ["name.desc"],
                search: ["name ct Some Project"]);
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                search:
                [
                    "name sw Some",
                    "is_private eq true",
                    "lifecycle neq Archived",
                    "latest_status eq OnTrack",
                    "last_activity gte 2024-01-15",
                    "created_date_time lt 2024-06-30"
                ]);
            Add(query);

            query = ProjectQueryFactory.CreateGetCollectionProjectQuery(
                orderBy:
                [
                    "name.asc",
                    "last_activity.desc",
                    "created_date_time.asc",
                    "updated_date_time.desc"
                ]);
            Add(query);
        }
    }
}