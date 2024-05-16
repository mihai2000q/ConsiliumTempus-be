using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Queries.Get;
using ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectSprint;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectSprint
    {
        internal static bool AssertGetQuery(
            GetProjectSprintQuery query,
            GetProjectSprintRequest request)
        {
            query.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertGetCollectionQuery(
            GetCollectionProjectSprintQuery query,
            GetCollectionProjectSprintRequest request)
        {
            query.ProjectId.Should().Be(request.ProjectId);

            return true;
        }

        internal static bool AssertCreateCommand(
            CreateProjectSprintCommand command,
            CreateProjectSprintRequest request)
        {
            command.ProjectId.Should().Be(request.ProjectId);
            command.Name.Should().Be(request.Name);
            command.StartDate.Should().Be(request.StartDate);
            command.EndDate.Should().Be(request.EndDate);

            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateProjectSprintCommand command,
            UpdateProjectSprintRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.StartDate.Should().Be(request.StartDate);
            command.EndDate.Should().Be(request.EndDate);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectSprintCommand command,
            DeleteProjectSprintRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        internal static void AssertGetResponse(
            GetProjectSprintResponse response,
            ProjectSprintAggregate sprint)
        {
            response.Name.Should().Be(sprint.Name.Value);
            response.StartDate.Should().Be(sprint.StartDate);
            response.EndDate.Should().Be(sprint.EndDate);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectSprintResponse response,
            GetCollectionProjectSprintResult result)
        {
            response.Sprints.Zip(result.Sprints)
                .Should().AllSatisfy(p => AssertProjectSprintResponse(p.First, p.Second));
        }

        private static void AssertProjectSprintResponse(
            GetCollectionProjectSprintResponse.ProjectSprintResponse response,
            ProjectSprintAggregate projectSprint)
        {
            response.Id.Should().Be(projectSprint.Id.Value);
            response.Name.Should().Be(projectSprint.Name.Value);
            response.StartDate.Should().Be(projectSprint.StartDate);
            response.EndDate.Should().Be(projectSprint.EndDate);
        }
    }
}