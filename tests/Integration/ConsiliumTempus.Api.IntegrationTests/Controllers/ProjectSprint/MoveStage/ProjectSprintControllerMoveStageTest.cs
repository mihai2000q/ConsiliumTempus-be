using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.MoveStage;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.MoveStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerMoveStageTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task MoveStageFromProjectSprint_WhenSucceeds_ShouldMoveStageAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectSprintData.Users.First();
        var sprint = ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[0];
        var overStage = sprint.Stages[2];
        var request = ProjectSprintRequestFactory.CreateMoveStageFromProjectSprintRequest(
            id: sprint.Id.Value,
            stageId: stage.Id.Value,
            overStageId: overStage.Id.Value);

        var expectedCustomOrderPosition = overStage.CustomOrderPosition.Value;

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/sprints/Move-Stage", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<MoveStageFromProjectSprintResponse>();
        response!.Message.Should().Be("Stage has been successfully moved from Project Sprint!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var newSprint = await dbContext.ProjectSprints
            .AsNoTracking()
            .Include(ps => ps.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .Include(ps => ps.Project.Workspace)
            .SingleAsync(ps => ps.Id == sprint.Id);
        Utils.ProjectSprint.AssertMovedStage(newSprint, request, user, expectedCustomOrderPosition);
    }
    
    [Fact]
    public async Task MoveStageFromProjectSprint_WhenOverStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[0];
        var request = ProjectSprintRequestFactory.CreateMoveStageFromProjectSprintRequest(
            id: sprint.Id.Value,
            stageId: stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Put("api/projects/sprints/Move-Stage", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.NotFoundWithId(ProjectStageId.Create(request.OverStageId)));

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Set<ProjectStage>().SingleOrDefault(p => p.Id == ProjectStageId.Create(request.StageId))
            .Should().NotBeNull();
        dbContext.Set<ProjectStage>().SingleOrDefault(p => p.Id == ProjectStageId.Create(request.OverStageId))
            .Should().BeNull();
    }

    [Fact]
    public async Task MoveStageFromProjectSprint_WhenStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateMoveStageFromProjectSprintRequest(
            id: sprint.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Put("api/projects/sprints/Move-Stage", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.NotFoundWithId(ProjectStageId.Create(request.StageId)));

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Set<ProjectStage>().SingleOrDefault(p => p.Id == ProjectStageId.Create(request.StageId))
            .Should().BeNull();
    }

    [Fact]
    public async Task MoveStageFromProjectSprint_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateMoveStageFromProjectSprintRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/sprints/Move-Stage", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().BeNull();
    }
}