using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateFavorites;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.UpdateFavorites;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateFavoritesTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateFavoritesWorkspace_WhenItSucceeds_ShouldUpdateFavoritesAndReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateFavoritesWorkspaceRequest(id: workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/workspaces/Favorites", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateFavoritesWorkspaceResponse>();
        response!.Message.Should().Be("Workspace favorites have been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedWorkspace = dbContext.Workspaces
            .SingleOrDefault(u => u.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertUpdatedFavorites(updatedWorkspace!, request, user);
    }

    [Fact]
    public async Task UpdateFavoritesWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateFavoritesWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/workspaces/Favorites", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.SingleOrDefault(u => u.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}