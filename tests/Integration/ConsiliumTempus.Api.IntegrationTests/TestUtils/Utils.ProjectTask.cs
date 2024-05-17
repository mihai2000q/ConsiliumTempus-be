using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class ProjectTask
    {
        internal static void AssertGetResponse(
            GetProjectTaskResponse response,
            ProjectTaskAggregate sprint)
        {
            response.Name.Should().Be(sprint.Name.Value);
            response.Description.Should().Be(sprint.Description.Value);
        }
        
        internal static void AssertGetCollectionResponse(
            GetCollectionProjectTaskResponse response,
            IReadOnlyList<ProjectTaskAggregate> tasks,
            int totalCount)
        {
            response.TotalCount.Should().Be(totalCount);
            response.Tasks.Should().HaveCount(tasks.Count);
            response.Tasks.Zip(tasks)
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

            task.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            task.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
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

            stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
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
            newTask.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            newTask.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            newTask.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
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
            newTask.Assignee?.Id.Value.Should().Be(request.AssigneeId!.Value);
            newTask.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            newTask.Stage.Sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            newTask.Stage.Sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }
        
        private static void AssertResponse(
            GetCollectionProjectTaskResponse.ProjectTaskResponse response,
            ProjectTaskAggregate projectTask)
        {
            response.Id.Should().Be(projectTask.Id.Value);
            response.Name.Should().Be(projectTask.Name.Value);
        }
    }
}