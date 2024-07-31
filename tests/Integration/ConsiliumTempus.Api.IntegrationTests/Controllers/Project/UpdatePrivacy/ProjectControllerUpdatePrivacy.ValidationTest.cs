using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdatePrivacy;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdatePrivacyValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdatePrivacyProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdatePrivacyProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Put("api/projects/privacy", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdatePrivacyProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdatePrivacyProjectRequest(
            id: Guid.Empty);

        // Act
        var outcome = await Client.Put("api/projects/privacy", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}