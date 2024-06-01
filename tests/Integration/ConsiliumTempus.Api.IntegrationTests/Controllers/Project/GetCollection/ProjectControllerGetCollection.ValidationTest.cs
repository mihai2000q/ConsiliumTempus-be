using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollection;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetCollectionProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(orderBy: ["name.asc"]);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects?{request.OrderBy?.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            pageSize: -1);

        // Act
        var outcome = await Client.Get($"api/projects" +
                                       $"?pageSize={request.PageSize}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}