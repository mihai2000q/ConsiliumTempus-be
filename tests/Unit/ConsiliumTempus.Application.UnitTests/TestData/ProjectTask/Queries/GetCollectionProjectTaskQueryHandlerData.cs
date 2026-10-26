using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Queries;

internal static class GetCollectionProjectTaskQueryHandlerData
{
    internal class GetQueries : TheoryData<GetCollectionProjectTaskQuery>
    {
        public GetQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery();
            Add(query);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name ct Some Task"],
                pageSize: 12,
                currentPage: 1);
            Add(query);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                search: ["name sw Some", "name neq Some Task", "is_completed eq true"]);
            Add(query);

            query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(
                orderBy:
                [
                    "name.asc",
                    "is_completed.desc",
                    "created_date_time.asc",
                    "updated_date_time.desc"
                ]);
            Add(query);
        }
    }
}
