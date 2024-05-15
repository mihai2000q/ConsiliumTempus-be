using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.GetCollection;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerGetCollectionValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task GetCollectionProjectStage_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateGetCollectionProjectStageRequest(
            ProjectStageData.ProjectSprints.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectStageData.Users.First());
        var outcome = await Client.Get($"api/projects/stages" +
                                       $"?projectSprintId={request.ProjectSprintId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCollectionProjectStage_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateGetCollectionProjectStageRequest(Guid.Empty);

        // Act
        var outcome = await Client.Get($"api/projects/stages" +
                                       $"?projectSprintId={request.ProjectSprintId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}