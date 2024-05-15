using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateOverview;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateOverviewValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateOverviewProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var id = ProjectData.Projects.First().Id.Value;
        var request = ProjectRequestFactory.CreateUpdateOverviewProjectRequest(id);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Put("api/projects/overview", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateOverviewProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOverviewProjectRequest(
            id: Guid.Empty,
            description: string.Empty);

        // Act
        var outcome = await Client.Put("api/projects/overview", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}