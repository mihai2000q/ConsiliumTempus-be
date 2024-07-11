using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.MoveStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerMoveStageValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task MoveStageFromProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[1];
        var overStage = sprint.Stages[0];
        var request = ProjectSprintRequestFactory.CreateMoveStageFromProjectSprintRequest(
            id: sprint.Id.Value,
            stageId: stage.Id.Value,
            overStageId: overStage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Put("api/projects/sprints/Move-Stage", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MoveStageFromProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateMoveStageFromProjectSprintRequest(
            id: Guid.Empty, 
            stageId: Guid.Empty,
            overStageId: Guid.Empty);  

        // Act
        var outcome = await Client.Put("api/projects/sprints/Move-Stage", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}