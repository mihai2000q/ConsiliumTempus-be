using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.AddStatus;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerAddStatusValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task AddStatusToProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var projectId = ProjectData.Projects.First().Id.Value;
        var request = ProjectRequestFactory.CreateAddStatusToProjectRequest(projectId);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Post("api/projects/Add-Status", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddStatusToProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddStatusToProjectRequest(
            id: Guid.Empty,
            title: string.Empty);

        // Act
        var outcome = await Client.Post("api/projects/Add-Status", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}