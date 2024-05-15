using ConsiliumTempus.Application.Project.Entities.Sprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Sprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Sprint.Queries;

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