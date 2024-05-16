using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.UpdateStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerUpdateStageTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenSucceeds_ShouldUpdateStageAndReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[0];
        var request = ProjectSprintRequestFactory.CreateUpdateStageFromProjectSprintRequest(
            id: sprint.Id.Value,
            stageId: stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Put("api/projects/sprints/Update-Stage", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<AddStageToProjectSprintResponse>();
        response!.Message.Should().Be("Stage has been successfully updated from Project Sprint!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var newSprint = await dbContext.ProjectSprints
            .Include(ps => ps.Stages)
            .Include(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleAsync(ps => ps.Id == sprint.Id);
        Utils.ProjectSprint.AssertUpdatedStage(newSprint, request);
    }
    
    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenStageIsNotFound_ShouldReturnStageNotFoundError()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateUpdateStageFromProjectSprintRequest(
            id: sprint.Id.Value);

        // Act
        var outcome = await Client.Put("api/projects/sprints/Update-Stage", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.ProjectStages.SingleOrDefault(p => p.Id == ProjectStageId.Create(request.StageId))
            .Should().BeNull();
    }

    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateStageFromProjectSprintRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/sprints/Update-Stage", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().BeNull();
    }
}