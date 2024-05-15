using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Common.UnitTests.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Queries.Get;

internal static class GetProjectQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetProjectQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectQueryFactory.CreateGetProjectQuery();
            Add(query);

            query = new GetProjectQuery(Guid.NewGuid());
            Add(query);
        }
    }

    internal class GetInvalidIdQueries : TheoryData<GetProjectQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = ProjectQueryFactory.CreateGetProjectQuery(id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}