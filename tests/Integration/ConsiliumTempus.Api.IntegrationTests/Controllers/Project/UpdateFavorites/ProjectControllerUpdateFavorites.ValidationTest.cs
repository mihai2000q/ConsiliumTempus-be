using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateFavorites;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateFavoritesValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateFavoritesProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var id = ProjectData.Projects.First().Id.Value;
        var request = ProjectRequestFactory.CreateUpdateFavoritesProjectRequest(id);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Put("api/projects/Favorites", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateFavoritesProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateFavoritesProjectRequest(
            id: Guid.Empty);

        // Act
        var outcome = await Client.Put("api/projects/Favorites", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}