using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetOverview;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static bool AssertGetProjectQuery(
            GetProjectQuery query,
            GetProjectRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetOverviewProjectQuery(
            GetOverviewProjectQuery query,
            GetOverviewProjectRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetCollectionProjectQuery(
            GetCollectionProjectQuery query,
            GetCollectionProjectRequest request)
        {
            query.PageSize.Should().Be(request.PageSize);
            query.CurrentPage.Should().Be(request.CurrentPage);
            query.Order.Should().Be(request.Order);
            query.WorkspaceId.Should().Be(request.WorkspaceId);
            query.Name.Should().Be(request.Name);
            query.IsFavorite.Should().Be(request.IsFavorite);
            query.IsPrivate.Should().Be(request.IsPrivate);

            return true;
        }

        internal static bool AssertCreateCommand(
            CreateProjectCommand command,
            CreateProjectRequest request)
        {
            command.WorkspaceId.Should().Be(request.WorkspaceId);
            command.Name.Should().Be(request.Name);
            command.IsPrivate.Should().Be(request.IsPrivate);

            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateProjectCommand command,
            UpdateProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.IsFavorite.Should().Be(request.IsFavorite);

            return true;
        }
        
        internal static bool AssertUpdateOverviewCommand(
            UpdateOverviewProjectCommand command,
            UpdateOverviewProjectRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Description.Should().Be(request.Description);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectCommand command,
            DeleteProjectRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        internal static void AssertGetProjectResponse(
            GetProjectResponse response,
            ProjectAggregate project)
        {
            response.Name.Should().Be(project.Name.Value);
            response.Description.Should().Be(project.Description.Value);
            response.IsFavorite.Should().Be(project.IsFavorite.Value);
            response.IsPrivate.Should().Be(project.IsPrivate.Value);
        }

        internal static void AssertGetOverviewProjectResponse(
            GetOverviewProjectResponse response,
            GetOverviewProjectResult result)
        {
            response.Description.Should().Be(result.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectResponse response,
            GetCollectionProjectResult result)
        {
            response.Projects.Zip(result.Projects)
                .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second));
            response.TotalCount.Should().Be(result.TotalCount);
            response.TotalPages.Should().Be(result.TotalPages);
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