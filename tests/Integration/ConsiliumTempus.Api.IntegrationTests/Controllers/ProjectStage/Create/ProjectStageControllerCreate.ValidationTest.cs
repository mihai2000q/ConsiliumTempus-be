using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.Create;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerCreateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task CreateProjectStage_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectStageData.ProjectSprints.First();
        var request = ProjectStageRequestFactory.CreateCreateProjectStageRequest(sprint.Id.Value);

        // Act
        Client.UseCustomToken(ProjectStageData.Users.First());
        var outcome = await Client.Post("api/projects/stages", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateProjectStage_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateCreateProjectStageRequest(
            projectSprintId: Guid.Empty, 
            name: string.Empty);  

        // Act
        var outcome = await Client.Post("api/projects/stages", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}