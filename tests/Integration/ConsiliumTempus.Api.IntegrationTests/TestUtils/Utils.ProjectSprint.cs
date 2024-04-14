using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectSprint
    {
        internal static void AssertGetCollectionResponse(
            GetCollectionProjectSprintResponse response,
            IEnumerable<Domain.Project.Entities.ProjectSprint> sprints)
        {
            response.Sprints
                .Zip(sprints)
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
        }

        internal static void AssertCreation(
            Domain.Project.Entities.ProjectSprint sprint,
            CreateProjectSprintRequest request)
        {
            sprint.Name.Value.Should().Be(request.Name);
            sprint.StartDate.Should().Be(request.StartDate);
            sprint.EndDate.Should().Be(request.EndDate);
            sprint.Project.Id.Value.Should().Be(request.ProjectId);

            sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }

        private static void AssertResponse(
            GetCollectionProjectSprintResponse.ProjectSprintResponse response,
            Domain.Project.Entities.ProjectSprint projectSprint)
        {
            response.Id.Should().Be(projectSprint.Id.Value);
            response.Name.Should().Be(projectSprint.Name.Value);
            response.StartDate.Should().Be(projectSprint.StartDate);
            response.EndDate.Should().Be(projectSprint.EndDate);
        }
    }
}