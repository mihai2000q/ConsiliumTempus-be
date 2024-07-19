using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Queries.GetCollection;

internal static class GetCollectionProjectSprintQueryHandlerData
{
    internal class GetQueriesAndSprints : TheoryData<GetCollectionProjectSprintQuery, List<ProjectSprintAggregate>>
    {
        public GetQueriesAndSprints()
        {
            var sprints = ProjectSprintFactory.CreateList();
            var query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery();
            Add(query, sprints);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name ct something"]);
            Add(query, sprints);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                search: ["name ct something"],
                fromThisYear: true);
            Add(query, sprints);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                fromThisYear: true);
            Add(query, sprints);

            query = ProjectSprintQueryFactory.CreateGetCollectionProjectSprintQuery(
                fromThisYear: true);
            Add(query, []);
        }
    }
}