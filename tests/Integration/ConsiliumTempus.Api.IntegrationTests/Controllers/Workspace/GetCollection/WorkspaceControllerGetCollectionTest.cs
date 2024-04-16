using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetCollection;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetCollectionTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetWorkspaceCollectionForUser_WhenIsSuccessful_ShouldReturnAllTheWorkspacesForUser()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var workspaces = user.Memberships.Select(m => m.Workspace)
            .OrderBy(w => w.Name.Value)
            .ToList();

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get("api/Workspaces");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionWorkspaceResponse>();
        Utils.Workspace.AssertGetCollectionResponse(response!, workspaces);
    }

    [Fact]
    public async Task GetWorkspaceCollectionForUser_WhenUserIsNotFound_ShouldReturnUserNotFoundError()
    {
        // Arrange

        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Get("api/Workspaces");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}