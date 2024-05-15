using ConsiliumTempus.Application.Project.Entities.Stage.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.Project.Entities.Stage;

namespace ConsiliumTempus.Application.UnitTests.TestData.Project.Entities.Stage.Queries;

internal static class GetCollectionProjectStageQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetCollectionProjectStageQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectStageQueryFactory.CreateGetCollectionProjectStageQuery();
            Add(query);

            query = new GetCollectionProjectStageQuery(Guid.NewGuid());
            Add(query);
        }
    }

    internal class GetInvalidProjectSprintIdQueries : TheoryData<GetCollectionProjectStageQuery, string>
    {
        public GetInvalidProjectSprintIdQueries()
        {
            var query = ProjectStageQueryFactory.CreateGetCollectionProjectStageQuery(
                projectSprintId: Guid.Empty);
            Add(query, nameof(query.ProjectSprintId));
        }
    }
}