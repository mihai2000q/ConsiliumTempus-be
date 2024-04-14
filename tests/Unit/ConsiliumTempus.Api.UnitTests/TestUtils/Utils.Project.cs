using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForWorkspace;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

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

        internal static void AssertGetCollectionForWorkspaceResponse(
            GetCollectionProjectForWorkspaceResponse response,
            GetCollectionProjectForWorkspaceResult result)
        {
            response.Projects
                .Zip(result.Projects)
                .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second));
        }

        internal static void AssertGetCollectionForUserResponse(
            GetCollectionProjectForUserResponse response,
            GetCollectionProjectForUserResult result)
        {
            response.Projects
                .Zip(result.Projects)
                .Should().AllSatisfy(p => AssertProjectResponse(p.First, p.Second));
        }

        internal static bool AssertGetProjectQuery(
            GetProjectQuery query,
            GetProjectRequest request)
        {
            query.Id.Should().Be(request.Id);
            return true;
        }

        internal static bool AssertGetCollectionProjectForWorkspaceQuery(
            GetCollectionProjectForWorkspaceQuery query,
            GetCollectionProjectForWorkspaceRequest request)
        {
            query.WorkspaceId.Should().Be(request.WorkspaceId);
            return true;
        }

        internal static bool AssertCreateCommand(CreateProjectCommand command, CreateProjectRequest request)
        {
            command.WorkspaceId.Should().Be(request.WorkspaceId);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.IsPrivate.Should().Be(request.IsPrivate);
            return true;
        }

        internal static bool AssertDeleteCommand(DeleteProjectCommand command, Guid id)
        {
            command.Id.Should().Be(id);
            return true;
        }

        private static void AssertProjectResponse(
            GetCollectionProjectForUserResponse.ProjectResponse projectResponse,
            ProjectAggregate project)
        {
            projectResponse.Id.Should().Be(project.Id.Value);
            projectResponse.Name.Should().Be(project.Name.Value);
        }

        private static void AssertProjectResponse(
            GetCollectionProjectForWorkspaceResponse.ProjectResponse projectResponse,
            ProjectAggregate project)
        {
            projectResponse.Id.Should().Be(project.Id.Value);
            projectResponse.Name.Should().Be(project.Name.Value);
            projectResponse.Description.Should().Be(project.Description.Value);
        }
    }

    internal static class ProjectSprint
    {
        internal static bool AssertCreateCommand(CreateProjectSprintCommand command, CreateProjectSprintRequest request)
        {
            command.ProjectId.Should().Be(request.ProjectId);
            command.Name.Should().Be(request.Name);
            command.StartDate.Should().Be(request.StartDate);
            command.EndDate.Should().Be(request.EndDate);
            return true;
        }

        internal static bool AssertDeleteCommand(DeleteProjectSprintCommand command, Guid id)
        {
            command.Id.Should().Be(id);
            return true;
        }
    }
}