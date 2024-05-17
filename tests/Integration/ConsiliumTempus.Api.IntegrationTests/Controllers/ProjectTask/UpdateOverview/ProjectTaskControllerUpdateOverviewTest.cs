using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.UpdateOverview;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerUpdateOverviewOverviewTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task UpdateOverviewProjectTask_WhenSucceeds_ShouldUpdateOverviewAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateUpdateOverviewProjectTaskRequest(task.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/overview", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, task, request);
    }
    
    [Fact]
    public async Task UpdateOverviewProjectTask_WhenRequestHasAssignee_ShouldUpdateOverviewAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var assignee = ProjectTaskData.Users[3];
        var request = ProjectTaskRequestFactory.CreateUpdateOverviewProjectTaskRequest(
            task.Id.Value,
            assigneeId: assignee.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/overview", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, task, request);
    }

    [Fact]
    public async Task UpdateOverviewProjectTask_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateOverviewProjectTaskRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/tasks/overview", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.Id))
            .Should().BeNull();
    }

    private async Task AssertSuccess(
        HttpResponseMessage outcome,
        ProjectTaskAggregate task,
        UpdateOverviewProjectTaskRequest request)
    {
        var response = await outcome.Content.ReadFromJsonAsync<UpdateOverviewProjectTaskResponse>();
        response!.Message.Should().Be("Project Task Overview has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedTask = await dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .Include(t => t.Stage.Tasks)
            .SingleAsync(t => t.Name.Value == request.Name);
        Utils.ProjectTask.AssertUpdatedOverview(updatedTask, task, request);
    }
}