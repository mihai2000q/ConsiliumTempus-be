using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.Update;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerUpdateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task UpdateProjectStage_WhenSuccessful_ShouldUpdateAndReturnSuccessfulResponse()
    {
        // Arrange
        var stage = ProjectStageData.ProjectStages.First();
        var request = ProjectStageRequestFactory.CreateUpdateProjectStageRequest(
            stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectStageData.Users.First());
        var outcome = await Client.Put("api/projects/stages", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateProjectStageResponse>();
        response!.Message.Should().Be("Project Stage has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var newStage = dbContext.ProjectStages
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .AsEnumerable()
            .Single(ps => ps.Id.Value == request.Id);
        
        Utils.ProjectStage.AssertUpdated(
            stage,
            newStage,
            request);
    }

    [Fact]
    public async Task UpdateProjectStage_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateUpdateProjectStageRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/stages", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectStage.NotFound);
    }
}