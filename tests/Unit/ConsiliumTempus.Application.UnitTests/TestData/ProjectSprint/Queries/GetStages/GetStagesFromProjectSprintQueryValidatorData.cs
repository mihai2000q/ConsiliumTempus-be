using ConsiliumTempus.Application.ProjectSprint.Queries.GetStages;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries.GetStages;

internal static class GetStagesFromProjectSprintQueryValidatorData
{
    internal class GetValidQueries : TheoryData<GetStagesFromProjectSprintQuery>
    {
        public GetValidQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetStagesFromProjectSprintQuery();
            Add(query);

            query = new GetStagesFromProjectSprintQuery(Guid.NewGuid());
            Add(query);
        }
    } 
    
    internal class GetInvalidIdQueries : TheoryData<GetStagesFromProjectSprintQuery, string>
    {
        public GetInvalidIdQueries()
        {
            var query = ProjectSprintQueryFactory.CreateGetStagesFromProjectSprintQuery(
                id: Guid.Empty);
            Add(query, nameof(query.Id));
        }
    }
}