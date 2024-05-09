using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.Delete;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerDeleteTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task DeleteProjectStage_WhenSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        var stage = ProjectStageData.ProjectStages.First();

        // Act
        Client.UseCustomToken(ProjectStageData.Users.First());
        var outcome = await Client.Delete($"api/projects/stages/{stage.Id.Value}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectStageResponse>();
        response!.Message.Should().Be("Project Stage has been deleted successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectStages.Should().HaveCount(ProjectStageData.ProjectStages.Length - 1);
        (await dbContext.ProjectStages.FindAsync(stage.Id))
            .Should().BeNull();
        
        var sprint = dbContext.ProjectSprints
            .Include(ps => ps.Stages)
            .Include(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .Single(p => p == stage.Sprint);
        
        sprint.Stages.Should().HaveCount(stage.Sprint.Stages.Count - 1);
        var order = 0;
        sprint.Stages.OrderBy(s => s.CustomOrderPosition.Value)
            .Should().AllSatisfy(s => s.CustomOrderPosition.Value.Should().Be(order++));
        
        sprint.Project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        sprint.Project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }

    [Fact]
    public async Task DeleteProjectStage_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var outcome = await Client.Delete($"api/projects/stages/{id}");

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectStages.Should().HaveCount(ProjectStageData.ProjectStages.Length);
        dbContext.ProjectStages.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == id)
            .Should().BeNull();
    }
}