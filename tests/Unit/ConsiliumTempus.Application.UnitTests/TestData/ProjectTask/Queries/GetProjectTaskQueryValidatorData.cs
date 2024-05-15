using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Queries;

internal static class GetProjectTaskQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetProjectTaskQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetProjectTaskQuery();
            Add(query);

            query = new GetProjectTaskQuery(Guid.NewGuid());
            Add(query);
        }
    }
    
    internal class GetInvalidIdQueries : TheoryData<GetProjectTaskQuery, string, short>
    {
        public GetInvalidIdQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetProjectTaskQuery(id: Guid.Empty);
            Add(query, nameof(query.Id), 1);
        }
    }
}