using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ErrorOr;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.UpdateFavorites;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateFavoritesAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateFavoritesWorkspace_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(WorkspaceData.Users[0]);
    }

    [Fact]
    public async Task UpdateFavoritesWorkspace_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(WorkspaceData.Users[3]);
    }

    [Fact]
    public async Task UpdateFavoritesWorkspace_WhenWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(WorkspaceData.Users[4]);
    }

    [Fact]
    public async Task UpdateFavoritesWorkspace_WhenWithoutMembership_ShouldReturnNotFoundError()
    {
        await AssertNotFoundResponse(Errors.Workspace.NotFound, WorkspaceData.Users[1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task AssertNotFoundResponse(Error error, UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        await outcome.ValidateError(error);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user)
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateFavoritesWorkspaceRequest(id: workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/workspaces/Favorites", request);
    }
}