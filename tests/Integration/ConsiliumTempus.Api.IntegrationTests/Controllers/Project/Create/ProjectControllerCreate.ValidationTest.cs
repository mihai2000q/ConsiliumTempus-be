using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Create;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerCreateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task CreateProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workspaceId = ProjectData.Workspaces.First().Id.Value;
        var request = ProjectRequestFactory.CreateCreateProjectRequest(workspaceId);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Post("api/projects", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest(
            workspaceId: Guid.Empty,
            name: string.Empty);

        // Act
        var outcome = await Client.Post("api/projects", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}