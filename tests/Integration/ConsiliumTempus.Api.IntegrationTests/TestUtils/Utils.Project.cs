using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project;
using FluentAssertions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static void AssertCreation(ProjectAggregate project, CreateProjectRequest request)
        {
            project.Name.Should().Be(request.Name);
            project.Description.Should().Be(request.Description);
            project.IsPrivate.Should().Be(request.IsPrivate);
            project.Workspace.Id.Value.Should().Be(request.WorkspaceId);

            project.Sprints.Should().HaveCount(1);
            project.Sprints[0].Sections.Should().HaveCount(Constants.ProjectSection.Names.Length);
            project.Sprints[0].Sections[0].Tasks
                .Should().HaveCount(Constants.ProjectTask.Names.Length);
        }
    }
}