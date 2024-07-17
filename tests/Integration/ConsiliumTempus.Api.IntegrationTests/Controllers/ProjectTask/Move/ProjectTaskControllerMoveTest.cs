using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.Move;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Move;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerMoveTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task MoveProjectTask_WhenMovingWithinStageOnUpperPosition_ShouldMoveAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks[1];
        var overTask = ProjectTaskData.ProjectTasks[3];
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            task.Stage.Sprint.Id.Value,
            task.Id.Value,
            overTask.Id.Value);

        var expectedCustomOrderPosition = overTask.CustomOrderPosition.Value;

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/Move", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<MoveProjectTaskResponse>();
        response!.Message.Should().Be("Project Task has been moved successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var movedTask = dbContext.Set<ProjectStage>()
            .AsNoTracking()
            .Include(s => s.Tasks.OrderBy(tt => tt.CustomOrderPosition.Value))
            .Include(s => s.Sprint.Project.Workspace)
            .Where(s => s.Sprint.Id == task.Stage.Sprint.Id)
            .ToList()
            .SelectMany(s => s.Tasks)
            .Single(t => t.Id == task.Id);

        Utils.ProjectTask.AssertMoveWithinStage(request, movedTask, expectedCustomOrderPosition);
    }

    [Fact]
    public async Task MoveProjectTask_WhenMovingWithinStageOnLowerPosition_ShouldMoveAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks[3];
        var overTask = ProjectTaskData.ProjectTasks[1];
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            task.Stage.Sprint.Id.Value,
            task.Id.Value,
            overTask.Id.Value);

        var expectedCustomOrderPosition = overTask.CustomOrderPosition.Value;

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/Move", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<MoveProjectTaskResponse>();
        response!.Message.Should().Be("Project Task has been moved successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var movedTask = dbContext.Set<ProjectStage>()
            .AsNoTracking()
            .Include(s => s.Tasks.OrderBy(tt => tt.CustomOrderPosition.Value))
            .Include(s => s.Sprint.Project.Workspace)
            .Where(s => s.Sprint.Id == task.Stage.Sprint.Id)
            .ToList()
            .SelectMany(s => s.Tasks)
            .Single(t => t.Id == task.Id);

        Utils.ProjectTask.AssertMoveWithinStage(request, movedTask, expectedCustomOrderPosition);
    }

    [Fact]
    public async Task MoveProjectTask_WhenMovingToAnotherStage_ShouldMoveAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks[1];
        var overStage = ProjectTaskData.ProjectStages[2];
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            task.Stage.Sprint.Id.Value,
            task.Id.Value,
            overStage.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/Move", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<MoveProjectTaskResponse>();
        response!.Message.Should().Be("Project Task has been moved successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var movedTask = await dbContext.ProjectTasks
            .AsNoTracking()
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleAsync(t => t.Id == task.Id);

        var stages = await dbContext.Set<ProjectStage>()
            .AsNoTracking()
            .Include(s => s.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .Where(s => s.Sprint.Id == task.Stage.Sprint.Id)
            .ToListAsync();

        Utils.ProjectTask.AssertMoveToAnotherStage(request, movedTask, stages);
    }

    [Fact]
    public async Task MoveProjectTask_WhenMovingOverTaskToAnotherStage_ShouldMoveAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks[0];
        var overTask = ProjectTaskData.ProjectTasks[5];
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            task.Stage.Sprint.Id.Value,
            task.Id.Value,
            overTask.Id.Value);

        var expectedCustomOrderPosition = overTask.CustomOrderPosition.Value;

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/Move", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<MoveProjectTaskResponse>();
        response!.Message.Should().Be("Project Task has been moved successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var movedTask = await dbContext.ProjectTasks
            .AsNoTracking()
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleAsync(t => t.Id == task.Id);

        var stages = await dbContext.Set<ProjectStage>()
            .AsNoTracking()
            .Include(s => s.Tasks.OrderBy(t => t.CustomOrderPosition.Value))
            .Where(s => s.Sprint.Id == movedTask.Stage.Sprint.Id)
            .ToListAsync();

        Utils.ProjectTask.AssertMoveOverTaskToAnotherStage(
            request, 
            movedTask, 
            overTask.Stage, 
            stages,
            expectedCustomOrderPosition);
    }

    [Fact]
    public async Task MoveProjectTask_WhenOverIsNotFound_ShouldReturnOverNotFoundError()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            task.Stage.Sprint.Id.Value,
            task.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/Move", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.OverNotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(t => t.Id == ProjectSprintId.Create(request.SprintId))
            .Should().NotBeNull();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.OverId))
            .Should().BeNull();
        dbContext.Set<ProjectStage>().SingleOrDefault(s => s.Id == ProjectStageId.Create(request.OverId))
            .Should().BeNull();
    }

    [Fact]
    public async Task MoveProjectTask_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var sprint = ProjectTaskData.ProjectSprints.First();
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(sprint.Id.Value);

        // Act
        var outcome = await Client.Put("api/projects/tasks/Move", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(t => t.Id == ProjectSprintId.Create(request.SprintId))
            .Should().NotBeNull();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.Id))
            .Should().BeNull();
    }

    [Fact]
    public async Task MoveProjectTask_WhenSprintIsNotFound_ShouldReturnSprintNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/tasks/Move", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(t => t.Id == ProjectSprintId.Create(request.SprintId))
            .Should().BeNull();
    }
}