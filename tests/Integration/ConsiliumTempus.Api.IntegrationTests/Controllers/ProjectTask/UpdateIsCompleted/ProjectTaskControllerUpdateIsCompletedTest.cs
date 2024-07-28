using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateIsCompleted;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.UpdateIsCompleted;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerUpdateIsCompletedTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task UpdateIsCompletedProjectTask_WhenSucceeds_ShouldUpdateIsCompletedAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateUpdateIsCompletedProjectTaskRequest(task.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/is-completed", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateIsCompletedProjectTaskResponse>();
        response!.Message.Should().Be("Project Task's completion status has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedTask = await dbContext.ProjectTasks
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleAsync(t => t.Id == ProjectTaskId.Create(request.Id));
        Utils.ProjectTask.AssertUpdatedIsCompleted(updatedTask, request);
    }

    [Fact]
    public async Task UpdateIsCompletedProjectTask_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateIsCompletedProjectTaskRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/tasks/is-completed", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.Id))
            .Should().BeNull();
    }
}