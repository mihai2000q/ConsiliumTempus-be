using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static void AssertGetResponse(
            GetProjectTaskResponse response,
            ProjectTaskAggregate task)
        {
            response.Name.Should().Be(task.Name.Value);
            response.Description.Should().Be(task.Description.Value);
            response.IsCompleted.Should().Be(task.IsCompleted.Value);
            AssertUserResponse(response.Assignee, task.Assignee);
            AssertProjectStageResponse(response.Stage, task.Stage);
            AssertProjectSprintResponse(response.Sprint, task.Stage.Sprint);
            AssertProjectResponse(response.Project, task.Stage.Sprint.Project);
            AssertWorkspaceResponse(response.Workspace, task.Stage.Sprint.Project.Workspace);
        }

        internal static void AssertGetCollectionResponse(
            GetCollectionProjectTaskResponse response,
            IReadOnlyList<ProjectTaskAggregate> tasks,
            int totalCount)
        {
            response.TotalCount.Should().Be(totalCount);
            response.Tasks.Should().HaveCount(tasks.Count);
            response.Tasks
                .Zip(tasks)
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
        }

        internal static void AssertCreation(
            ProjectTaskAggregate task,
            CreateProjectTaskRequest request,
            UserAggregate user)
        {
            task.Name.Value.Should().Be(request.Name);
            task.CustomOrderPosition.Value.Should().Be(request.OnTop ? 0 : task.Stage.Tasks.Count - 1);
            task.CreatedBy.Should().Be(user);
            task.Assignee.Should().BeNull();
            task.Stage.Id.Value.Should().Be(request.ProjectStageId);

            var customOrderPosition = 0;
            task.Stage.Tasks
                .OrderBy(ta => ta.CustomOrderPosition.Value)
                .Should().AllSatisfy(t => t.CustomOrderPosition.Value.Should().Be(customOrderPosition++));

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertDelete(
            ProjectStage stage,
            DeleteProjectTaskRequest request)
        {
            stage.Tasks.Should().NotContain(s => s.Id.Value == request.Id);

            var customOrderPosition = 0;
            stage.Tasks
                .OrderBy(s => s.CustomOrderPosition.Value)
                .Should().AllSatisfy(s => s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdated(
            ProjectTaskAggregate newTask,
            ProjectTaskAggregate task,
            UpdateProjectTaskRequest request)
        {
            // unchanged
            newTask.Id.Value.Should().Be(request.Id);
            newTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
            newTask.Stage.Should().Be(task.Stage);

            // changed
            newTask.Name.Value.Should().Be(request.Name);
            newTask.IsCompleted.Value.Should().Be(request.IsCompleted);
            newTask.Assignee?.Id.Value.Should().Be(request.AssigneeId!.Value);
            newTask.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newTask.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newTask.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal static void AssertUpdatedOverview(
            ProjectTaskAggregate newTask,
            ProjectTaskAggregate task,
            UpdateOverviewProjectTaskRequest request)
        {
            // unchanged
            newTask.Id.Value.Should().Be(request.Id);
            newTask.CreatedDateTime.Should().Be(task.CreatedDateTime);
            newTask.Stage.Should().Be(task.Stage);

            // changed
            newTask.Name.Value.Should().Be(request.Name);
            newTask.Description.Value.Should().Be(request.Description);
            newTask.IsCompleted.Value.Should().Be(request.IsCompleted);
            if (request.AssigneeId is null)
                newTask.Assignee.Should().BeNull();
            else
                newTask.Assignee!.Id.Value.Should().Be(request.AssigneeId.Value);
            newTask.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);

            newTask.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            newTask.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        private static void AssertUserResponse(
            GetProjectTaskResponse.UserResponse? response,
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

        private static void AssertProjectStageResponse(
            GetProjectTaskResponse.ProjectStageResponse response,
            ProjectStage stage)
        {
            response.Id.Should().Be(stage.Id.Value);
            response.Name.Should().Be(stage.Name.Value);
        }

        private static void AssertProjectSprintResponse(
            GetProjectTaskResponse.ProjectSprintResponse response,
            ProjectSprintAggregate sprint)
        {
            response.Id.Should().Be(sprint.Id.Value);
            response.Name.Should().Be(sprint.Name.Value);
            response.Stages.Zip(sprint.Stages)
                .Should().AllSatisfy(x => AssertProjectStageResponse(x.First, x.Second));
        }

        private static void AssertProjectResponse(
            GetProjectTaskResponse.ProjectResponse response,
            ProjectAggregate project)
        {
            response.Id.Should().Be(project.Id.Value);
            response.Name.Should().Be(project.Name.Value);
        }

        private static void AssertWorkspaceResponse(
            GetProjectTaskResponse.WorkspaceResponse response,
            WorkspaceAggregate workspace)
        {
            response.Id.Should().Be(workspace.Id.Value);
            response.Name.Should().Be(workspace.Name.Value);
        }

        private static void AssertResponse(
            GetCollectionProjectTaskResponse.ProjectTaskResponse response,
            ProjectTaskAggregate projectTask)
        {
            response.Id.Should().Be(projectTask.Id.Value);
            response.Name.Should().Be(projectTask.Name.Value);
            response.IsCompleted.Should().Be(projectTask.IsCompleted.Value);
            AssertUserResponse(response.Assignee, projectTask.Assignee);
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