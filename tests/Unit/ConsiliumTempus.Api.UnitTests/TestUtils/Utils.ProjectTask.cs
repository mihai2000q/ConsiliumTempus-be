using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static bool AssertGetProjectTaskQuery(
            GetProjectTaskQuery query,
            GetProjectTaskRequest request)
        {
            query.Id.Should().Be(request.Id);
            return true;
        }

        internal static bool AssertGetCollectionProjectTaskQuery(
            GetCollectionProjectTaskQuery query,
            GetCollectionProjectTaskRequest request)
        {
            query.ProjectStageId.Should().Be(request.ProjectStageId);

            return true;
        }

        internal static bool AssertCreateCommand(CreateProjectTaskCommand command, CreateProjectTaskRequest request)
        {
            command.ProjectStageId.Should().Be(request.ProjectStageId);
            command.Name.Should().Be(request.Name);
            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectTaskCommand command, 
            DeleteProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.StageId.Should().Be(request.StageId);
            
            return true;
        }

        internal static void AssertGetProjectTaskResponse(
            GetProjectTaskResponse response,
            ProjectTaskAggregate task)
        {
            response.Name.Should().Be(task.Name.Value);
            response.Description.Should().Be(task.Description.Value);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectTaskResponse response,
            GetCollectionProjectTaskResult result)
        {
            response.Tasks.Zip(result.Tasks)
                .Should().AllSatisfy(p => AssertProjectTaskResponse(p.First, p.Second));
            response.TotalCount.Should().Be(result.TotalCount);
        }

        private static void AssertProjectTaskResponse(
            GetCollectionProjectTaskResponse.ProjectTaskResponse taskResponse,
            ProjectTaskAggregate task)
        {
            taskResponse.Id.Should().Be(task.Id.Value);
            taskResponse.Name.Should().Be(task.Name.Value);
        }
    }
}