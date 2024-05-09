using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Sprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Get;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task GetProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetProjectSprintRequest(
            ProjectSprintData.ProjectSprints.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Get($"api/projects/sprints/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetProjectSprintRequest(Guid.Empty);

        // Act
        var outcome = await Client.Get($"api/projects/sprints/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}