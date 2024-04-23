using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static void AssertGetProjectResponse(
            GetProjectResponse response,
            ProjectAggregate project)
        {
            response.Name.Should().Be(project.Name.Value);
            response.Description.Should().Be(project.Description.Value);
            response.IsFavorite.Should().Be(project.IsFavorite.Value);
            response.IsPrivate.Should().Be(project.IsPrivate.Value);
        }

        internal static void AssertGetCollectionForUserResponse(
            GetCollectionProjectForUserResponse response,
            IEnumerable<ProjectAggregate> projects)
        {
            response.Projects
                .OrderBy(p => p.Id)
                .Zip(projects.OrderBy(p => p.Id.Value))
                .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second));
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectResponse response,
            IEnumerable<ProjectAggregate> projects)
        {
            response.Projects
                .OrderBy(p => p.Id)
                .Zip(projects.OrderBy(p => p.Id.Value))
                .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second));
        }

        internal static void AssertCreation(ProjectAggregate project, CreateProjectRequest request)
        {
            project.Name.Value.Should().Be(request.Name);
            project.Description.Value.Should().Be(request.Description);
            project.IsFavorite.Value.Should().Be(false);
            project.IsPrivate.Value.Should().Be(request.IsPrivate);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            project.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            project.Workspace.Id.Value.Should().Be(request.WorkspaceId);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            project.Sprints.Should().HaveCount(1);
            project.Sprints[0].Stages.Should().HaveCount(Constants.ProjectStage.Names.Length);
            project.Sprints[0].Stages[0].Tasks
                .Should().HaveCount(Constants.ProjectTask.Names.Length);
        }

        private static void AssertProjectResponse(
            GetCollectionProjectForUserResponse.ProjectResponse projectResponse,
            ProjectAggregate project)
        {
            projectResponse.Id.Should().Be(project.Id.Value);
            projectResponse.Name.Should().Be(project.Name.Value);
        }

        private static void AssertProjectResponse(
            GetCollectionProjectResponse.ProjectResponse projectResponse,
            ProjectAggregate project)
        {
            projectResponse.Id.Should().Be(project.Id.Value);
            projectResponse.Name.Should().Be(project.Name.Value);
            projectResponse.Description.Should().Be(project.Description.Value);
            projectResponse.IsFavorite.Should().Be(project.IsFavorite.Value);
            projectResponse.IsPrivate.Should().Be(project.IsPrivate.Value);
        }
    }
}