using ConsiliumTempus.Application.Project.Queries.GetStatuses;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.GetStatuses;

internal static class GetStatusesFromProjectQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetStatusesFromProjectQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectQueryFactory.CreateGetStatusesFromProjectQuery();
            Add(query);

            query = new GetStatusesFromProjectQuery(Guid.NewGuid());
            Add(query);
        }
    }

    internal class GetInvalidIdQueries : TheoryData<GetStatusesFromProjectQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = ProjectQueryFactory.CreateGetStatusesFromProjectQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}