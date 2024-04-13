using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectSprint
    {
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
    }
}