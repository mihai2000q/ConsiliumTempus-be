using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Create;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerCreateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task CreateProjectTask_WhenSucceeds_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest(stage.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/projects/tasks", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, request, user);
    }
    
    [Fact]
    public async Task CreateProjectTask_WhenRequestHasOnTop_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest(
            stage.Id.Value,
            onTop: true);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/projects/tasks", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, request, user);
    }

    [Fact]
    public async Task CreateProjectTask_WhenStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/projects/tasks", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.Should().HaveCount(ProjectTaskData.ProjectTasks.Length);
        dbContext.ProjectTasks.SingleOrDefault(t => t.Name.Value == request.Name)
            .Should().BeNull();
    }

    private async Task AssertSuccess(
        HttpResponseMessage outcome,
        CreateProjectTaskRequest request,
        UserAggregate user)
    {
        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectTaskResponse>();
        response!.Message.Should().Be("Project Task has been created successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.Should().HaveCount(ProjectTaskData.ProjectTasks.Length + 1);
        var createdTask = dbContext.Set<ProjectStage>()
            .AsNoTracking()
            .Include(s => s.Sprint.Project.Workspace)
            .Include(s => s.Tasks.OrderBy(tt => tt.CustomOrderPosition.Value))
            .ThenInclude(t => t.CreatedBy)
            .Single(s => s.Tasks.Any(t => t.Name.Value == request.Name))
            .Tasks
            .Single(t => t.Name.Value == request.Name);
        Utils.ProjectTask.AssertCreation(createdTask, request, user);
    }
}