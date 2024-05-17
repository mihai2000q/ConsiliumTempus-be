using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Delete;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerDeleteValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task DeleteProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateDeleteProjectSprintRequest(
            ProjectSprintData.ProjectSprints.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Delete($"api/projects/sprints/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateDeleteProjectSprintRequest(
            id: Guid.Empty);  

        // Act
        var outcome = await Client.Delete($"api/projects/sprints/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}