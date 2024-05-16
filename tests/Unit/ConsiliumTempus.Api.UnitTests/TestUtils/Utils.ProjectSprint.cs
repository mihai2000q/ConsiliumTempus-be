using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Delete;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.AddStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Application.ProjectSprint.Commands.Delete;
using ConsiliumTempus.Application.ProjectSprint.Commands.RemoveStage;
using ConsiliumTempus.Application.ProjectSprint.Commands.Update;
using ConsiliumTempus.Application.ProjectSprint.Commands.UpdateStage;
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

        internal static bool AssertAddStageCommand(
            AddStageToProjectSprintCommand command,
            AddStageToProjectSprintRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.OnTop.Should().Be(request.OnTop);

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

        internal static bool AssertUpdateStageCommand(
            UpdateStageFromProjectSprintCommand command,
            UpdateStageFromProjectSprintRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.StageId.Should().Be(request.StageId);
            command.Name.Should().Be(request.Name);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectSprintCommand command,
            DeleteProjectSprintRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        internal static bool AssertRemoveStageCommand(
            RemoveStageFromProjectSprintCommand command,
            RemoveStageFromProjectSprintRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.StageId.Should().Be(request.StageId);

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