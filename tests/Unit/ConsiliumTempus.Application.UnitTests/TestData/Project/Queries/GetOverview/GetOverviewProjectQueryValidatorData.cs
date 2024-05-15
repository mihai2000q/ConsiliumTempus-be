using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetOverview;

internal static class GetOverviewProjectQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetOverviewProjectQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectQueryFactory.CreateGetOverviewProjectQuery();
            Add(query);

            query = new GetOverviewProjectQuery(Guid.NewGuid());
            Add(query);
        }
    }

    internal class GetInvalidIdQueries : TheoryData<GetOverviewProjectQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = ProjectQueryFactory.CreateGetOverviewProjectQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}