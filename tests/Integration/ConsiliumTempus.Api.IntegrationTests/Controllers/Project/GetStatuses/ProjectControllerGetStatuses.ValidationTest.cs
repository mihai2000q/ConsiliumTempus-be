using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetStatuses;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetStatusesValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetStatusesFromProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetStatusesFromProjectRequest(
            ProjectData.Projects.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/{request.Id}/Statuses");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetStatusesFromProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetStatusesFromProjectRequest(Guid.Empty);

        // Act
        var outcome = await Client.Get($"api/projects/{request.Id}/Statuses");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}