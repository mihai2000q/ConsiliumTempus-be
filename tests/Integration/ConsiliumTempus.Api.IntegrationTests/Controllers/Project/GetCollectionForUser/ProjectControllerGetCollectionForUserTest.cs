using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollectionForUser;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionForUserTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetCollectionProjectForUser_WhenSucceeds_ShouldReturnProjects()
    {
        // Arrange
        var user = ProjectData.Users.First();

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get("api/projects/user");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectForUserResponse>();
        Utils.Project.AssertGetCollectionForUserResponse(
            response!,
            ProjectData.Projects
                .Where(p => p.Workspace.Memberships.Any(m => m.User == user)));
    }

    [Fact]
    public async Task GetCollectionProjectForUser_WhenUserIsNotFound_ShouldReturnUserNotFoundError()
    {
        // Arrange

        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Get("api/projects/user");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}