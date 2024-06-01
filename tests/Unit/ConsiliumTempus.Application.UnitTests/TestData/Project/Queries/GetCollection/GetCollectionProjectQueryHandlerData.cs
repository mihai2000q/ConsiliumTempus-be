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
        }
    }
}