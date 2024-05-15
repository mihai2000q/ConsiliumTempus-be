using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Update;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var id = ProjectData.Projects.First().Id.Value;
        var request = ProjectRequestFactory.CreateUpdateProjectRequest(id);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Put("api/projects", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateProjectRequest(
            id: Guid.Empty,
            name: string.Empty);

        // Act
        var outcome = await Client.Put("api/projects", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}