using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.RemoveStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerRemoveStageValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[0];
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest(
            sprint.Id.Value,
            stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Delete("api/projects/sprints" +
                                          $"/{request.Id}/Remove-Stage/{request.StageId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest(
            id: Guid.Empty,
            stageId: Guid.Empty);  

        // Act
        var outcome = await Client.Delete("api/projects/sprints" +
                                          $"/{request.Id}/Remove-Stage/{request.StageId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}