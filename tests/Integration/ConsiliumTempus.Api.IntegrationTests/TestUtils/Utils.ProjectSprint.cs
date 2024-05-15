using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Update;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectSprint
    {
        internal static void AssertGetResponse(
            GetProjectSprintResponse response,
            Domain.Project.Entities.ProjectSprint sprint)
        {
            response.Name.Should().Be(sprint.Name.Value);
            response.StartDate.Should().Be(sprint.StartDate);
            response.EndDate.Should().Be(sprint.EndDate);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectSprintResponse response,
            IReadOnlyList<Domain.Project.Entities.ProjectSprint> sprints)
        {
            response.Sprints.Should().HaveCount(sprints.Count);
            response.Sprints
                .Zip(sprints.OrderBy(s => s.StartDate)
                    .ThenBy(s => s.EndDate)
                    .ThenBy(s => s.Name.Value)
                    .ThenBy(s => s.CreatedDateTime))
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

        internal static void AssertUpdated(
            Domain.Project.Entities.ProjectSprint sprint,
            Domain.Project.Entities.ProjectSprint newSprint,
            UpdateProjectSprintRequest request)
        {
            // unchanged
            newSprint.Id.Value.Should().Be(request.Id);
            newSprint.CreatedDateTime.Should().Be(sprint.CreatedDateTime);
            newSprint.Project.Should().Be(sprint.Project);

            // changed
            newSprint.Name.Value.Should().Be(request.Name);
            newSprint.StartDate.Should().Be(request.StartDate);
            newSprint.EndDate.Should().Be(request.EndDate);
            newSprint.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            newSprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            newSprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
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