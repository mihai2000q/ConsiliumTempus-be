using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Update;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerUpdateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task UpdateProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateProjectSprintRequest(
            ProjectSprintData.ProjectSprints.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Put("api/projects/sprints", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateProjectSprintRequest(Guid.Empty);

        // Act
        var outcome = await Client.Put("api/projects/sprints", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}