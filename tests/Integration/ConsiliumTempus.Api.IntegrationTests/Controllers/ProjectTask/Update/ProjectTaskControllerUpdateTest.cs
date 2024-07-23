using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Update;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerUpdateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task UpdateProjectTask_WhenSucceeds_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateUpdateProjectTaskRequest(task.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, task, request);
    }
    
    [Fact]
    public async Task UpdateProjectTask_WhenRequestHasAssignee_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var assignee = ProjectTaskData.Users[3];
        var request = ProjectTaskRequestFactory.CreateUpdateProjectTaskRequest(
            task.Id.Value,
            assigneeId: assignee.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, task, request);
    }

    [Fact]
    public async Task UpdateProjectTask_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateProjectTaskRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/tasks", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.Id))
            .Should().BeNull();
    }

    private async Task AssertSuccess(
        HttpResponseMessage outcome,
        ProjectTaskAggregate task,
        UpdateProjectTaskRequest request)
    {
        var response = await outcome.Content.ReadFromJsonAsync<UpdateProjectTaskResponse>();
        response!.Message.Should().Be("Project Task has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedTask = await dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleAsync(t => t.Id == ProjectTaskId.Create(request.Id));
        Utils.ProjectTask.AssertUpdated(updatedTask, task, request);
    }
}