using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries.Get;

internal static class GetProjectSprintQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetProjectSprintQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetProjectSprintQuery();
            Add(query);

            query = new GetProjectSprintQuery(Guid.NewGuid());
            Add(query);
        }
    } 
    
    internal class GetInvalidIdQueries : TheoryData<GetProjectSprintQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetProjectSprintQuery(
                id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}