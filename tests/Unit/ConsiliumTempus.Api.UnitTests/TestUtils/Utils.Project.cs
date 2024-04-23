using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Queries.Get;
using ConsiliumTempus.Application.Project.Queries.GetCollection;
using ConsiliumTempus.Application.Project.Queries.GetCollectionForUser;
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

        internal static bool AssertGetCollectionProjectForUserQuery(
            GetCollectionProjectForUserQuery query)
        {
            query.Should().Be(new GetCollectionProjectForUserQuery());
            return true;
        }

        internal static bool AssertGetCollectionProjectQuery(
            GetCollectionProjectQuery query,
            GetCollectionProjectRequest request)
        {
            query.WorkspaceId.Should().Be(request.WorkspaceId);
            query.Name.Should().Be(request.Name);
            query.IsFavorite.Should().Be(request.IsFavorite);
            query.IsPrivate.Should().Be(request.IsPrivate);
            
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
            GetCollectionProjectResponse response,
            GetCollectionProjectResult result)
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