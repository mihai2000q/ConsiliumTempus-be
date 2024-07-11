using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.RemoveStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerRemoveStageTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenSucceeds_ShouldRemoveStageAndReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[0];
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest(
            id: sprint.Id.Value,
            stageId: stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Delete("api/projects/sprints" +
                                          $"/{request.Id}/Remove-Stage/{request.StageId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<RemoveStageFromProjectSprintResponse>();
        response!.Message.Should().Be("Stage has been successfully removed from Project Sprint!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<ProjectStage>().Should().HaveCount(ProjectSprintData.ProjectStages.Length - 1);
        var newSprint = await dbContext.ProjectSprints
            .Include(ps => ps.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .Include(ps => ps.Project.Workspace)
            .SingleAsync(ps => ps.Id == sprint.Id);
        Utils.ProjectSprint.AssertRemovedStage(newSprint, request);
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest(
            id: sprint.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Delete("api/projects/sprints" +
                                          $"/{request.Id}/Remove-Stage/{request.StageId}");

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Set<ProjectStage>().SingleOrDefault(p => p.Id == ProjectStageId.Create(request.StageId))
            .Should().BeNull();
    }
    
    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenThereIsOnlyOneStage_ShouldReturnOnlyOneStageError()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints[1];
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest(
            id: sprint.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Delete("api/projects/sprints" +
                                          $"/{request.Id}/Remove-Stage/{request.StageId}");

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.OnlyOneStage);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().NotBeNull();
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest();

        // Act
        var outcome = await Client.Delete("api/projects/sprints" +
                                          $"/{request.Id}/Remove-Stage/{request.StageId}");

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().BeNull();
    }
}