using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries;

internal static class GetCollectionProjectSprintQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectSprintQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery();
            Add(query);

            query = new GetCollectionProjectSprintQuery(
                Guid.NewGuid());
            Add(query);
        }
    } 
    
    internal class GetInvalidProjectIdQueries : TheoryData<GetCollectionProjectSprintQuery, string>
    {
        public GetInvalidProjectIdQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(projectId: Guid.Empty);
            const string field = nameof(query.ProjectId);
            Add(query, field);
        }
    } 
}