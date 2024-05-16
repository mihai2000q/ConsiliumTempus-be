using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.AddStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerAddStageValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task AddStageToProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateAddStageToProjectSprintRequest(sprint.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Post("api/projects/sprints/Add-Stage", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddStageToProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateAddStageToProjectSprintRequest(
            id: Guid.Empty, 
            name: string.Empty);  

        // Act
        var outcome = await Client.Post("api/projects/sprints/Add-Stage", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}