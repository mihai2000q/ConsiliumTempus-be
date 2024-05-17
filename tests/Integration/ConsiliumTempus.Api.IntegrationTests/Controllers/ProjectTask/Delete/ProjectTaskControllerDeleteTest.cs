using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Delete;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerDeleteTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task DeleteProjectTask_WhenSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateDeleteProjectTaskRequest(task.Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Delete($"api/projects/tasks/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectTaskResponse>();
        response!.Message.Should().Be("Project Task has been deleted successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.Should().HaveCount(ProjectTaskData.ProjectTasks.Length - 1);
        (await dbContext.ProjectTasks.FindAsync(task.Id))
            .Should().BeNull();
        var stage = await dbContext.Set<ProjectStage>()
            .Include(s => s.Sprint.Project.Workspace)
            .SingleAsync(s => s.Id == task.Stage.Id);
        Utils.ProjectTask.AssertDelete(stage, request);
    }

    [Fact]
    public async Task DeleteProjectTask_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateDeleteProjectTaskRequest();

        // Act
        var outcome = await Client.Delete($"api/projects/tasks/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.Should().HaveCount(ProjectTaskData.ProjectTasks.Length);
        dbContext.ProjectTasks.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == request.Id)
            .Should().BeNull();
    }
}