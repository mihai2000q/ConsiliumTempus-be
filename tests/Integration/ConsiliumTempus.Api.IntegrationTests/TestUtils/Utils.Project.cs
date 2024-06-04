using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.User;

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
            response.IsFavorite.Should().Be(project.IsFavorite.Value);
            response.IsPrivate.Should().Be(project.IsPrivate.Value);
        }

        internal static void AssertGetOverviewProjectResponse(
            GetOverviewProjectResponse response,
            ProjectAggregate project)
        {
            response.Description.Should().Be(project.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectResponse response,
            IReadOnlyList<ProjectAggregate> projects,
            int totalCount,
            int? totalPages,
            bool isOrdered = false)
        {
            response.Projects.Should().HaveCount(projects.Count);
            if (isOrdered)
            {
                response.Projects
                    .Zip(projects)
                    .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second));
            }
            else
            {
                response.Projects
                    .OrderBy(p => p.Id)
                    .Zip(projects.OrderBy(p => p.Id.Value))
                    .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second));
            }

            response.TotalCount.Should().Be(totalCount);
        }

        internal static void AssertCreation(
            ProjectAggregate project,
            CreateProjectRequest request,
            UserAggregate owner)
        {
            project.Name.Value.Should().Be(request.Name);
            project.Description.Value.Should().BeEmpty();
            project.IsFavorite.Value.Should().Be(false);
            project.IsPrivate.Value.Should().Be(request.IsPrivate);
            project.Owner.Should().Be(owner);
            project.Lifecycle.Should().Be(ProjectLifecycle.Active);
            project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            project.Statuses.Should().BeEmpty();

            project.Workspace.Id.Value.Should().Be(request.WorkspaceId);
            project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            project.Sprints.Should().HaveCount(1);
            project.Sprints[0].Stages.Should().HaveCount(Constants.ProjectStage.Names.Length);
            project.Sprints[0].Stages[0].Tasks
                .Should().HaveCount(Constants.ProjectTask.Names.Length);
        }

        internal static void AssertUpdate(
            ProjectAggregate project,
            ProjectAggregate newProject,
            UpdateProjectRequest request)
        {
            // unchanged
            newProject.Id.Value.Should().Be(request.Id);
            newProject.CreatedDateTime.Should().Be(project.CreatedDateTime);

            // changed
            newProject.Name.Value.Should().Be(request.Name);
            newProject.IsFavorite.Value.Should().Be(request.IsFavorite);
            newProject.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newProject.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newProject.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdateOverview(
            ProjectAggregate project,
            ProjectAggregate newProject,
            UpdateOverviewProjectRequest request)
        {
            // unchanged
            newProject.Id.Value.Should().Be(request.Id);
            newProject.CreatedDateTime.Should().Be(project.CreatedDateTime);

            // changed
            newProject.Description.Value.Should().Be(request.Description);
            newProject.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newProject.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newProject.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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