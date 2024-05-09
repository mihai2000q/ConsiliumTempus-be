using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.Update;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerUpdateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task UpdateProjectStage_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateUpdateProjectStageRequest(
            ProjectStageData.ProjectStages.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectStageData.Users.First());
        var outcome = await Client.Put("api/projects/stages", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateProjectStage_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateUpdateProjectStageRequest(Guid.Empty);

        // Act
        var outcome = await Client.Put("api/projects/stages", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}