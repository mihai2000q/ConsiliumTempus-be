using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Commands.Create;
using ConsiliumTempus.Application.ProjectTask.Commands.Delete;
using ConsiliumTempus.Application.ProjectTask.Commands.Update;
using ConsiliumTempus.Application.ProjectTask.Commands.UpdateOverview;
using ConsiliumTempus.Application.ProjectTask.Queries.Get;
using ConsiliumTempus.Application.ProjectTask.Queries.GetCollection;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;

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
            query.Search.Should().BeEquivalentTo(request.Search);
            query.OrderBy.Should().BeEquivalentTo(request.OrderBy);

            return true;
        }

        internal static bool AssertCreateCommand(
            CreateProjectTaskCommand command,
            CreateProjectTaskRequest request)
        {
            command.ProjectStageId.Should().Be(request.ProjectStageId);
            command.Name.Should().Be(request.Name);
            command.OnTop.Should().Be(request.OnTop);

            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateProjectTaskCommand command,
            UpdateProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.IsCompleted.Should().Be(request.IsCompleted);
            command.AssigneeId.Should().Be(request.AssigneeId);

            return true;
        }

        internal static bool AssertUpdateOverviewCommand(
            UpdateOverviewProjectTaskCommand command,
            UpdateOverviewProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.AssigneeId.Should().Be(request.AssigneeId);

            return true;
        }

        internal static bool AssertDeleteCommand(
            DeleteProjectTaskCommand command,
            DeleteProjectTaskRequest request)
        {
            command.Id.Should().Be(request.Id);

            return true;
        }

        internal static void AssertGetProjectTaskResponse(
            GetProjectTaskResponse response,
            ProjectTaskAggregate task)
        {
            response.Name.Should().Be(task.Name.Value);
            response.Description.Should().Be(task.Description.Value);
            response.IsCompleted.Should().Be(task.IsCompleted.Value);
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
            taskResponse.IsCompleted.Should().Be(task.IsCompleted.Value);
            AssertUserResponse(taskResponse.Assignee, task.Assignee);
        }

        private static void AssertUserResponse(
            GetCollectionProjectTaskResponse.UserResponse? response,
            UserAggregate? user)
        {
            if (user is null)
            {
                response.Should().BeNull();
                return;
            }

            response!.Id.Should().Be(user.Id.Value);
            response.Name.Should().Be(user.FirstName.Value + " " + user.LastName.Value);
            response.Email.Should().Be(user.Credentials.Email);
        }
    }
}