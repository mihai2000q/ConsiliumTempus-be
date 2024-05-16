using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectTask;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectTask.Queries;

internal static class GetCollectionProjectTaskQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectTaskQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery();
            Add(query);

            query = new GetCollectionProjectTaskQuery(
                Guid.NewGuid());
            Add(query);
        }
    }
    
    internal class GetInvalidProjectStageIdQueries : TheoryData<GetCollectionProjectTaskQuery, string>
    {
        public GetInvalidProjectStageIdQueries()
        {
            var query = ProjectTaskQueryFactory.CreateGetCollectionProjectTaskQuery(Guid.Empty);
            Add(query, nameof(query.ProjectStageId));
        }
    }
}