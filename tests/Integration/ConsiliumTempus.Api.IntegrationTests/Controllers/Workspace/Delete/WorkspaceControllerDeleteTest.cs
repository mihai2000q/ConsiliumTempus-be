using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Delete;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerDeleteTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task WorkspaceDelete_WhenItSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces[0];

        // Act
        Client.UseCustomToken(WorkspaceData.Users[0]);
        var outcome = await Client.Delete($"api/workspaces/{workspace.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteWorkspaceResponse>();
        response!.Message.Should().Be("Workspace has been deleted successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(WorkspaceData.Workspaces.Length - 1);
        (await dbContext.Workspaces.FindAsync(workspace.Id))
            .Should().BeNull();
    }

    [Fact]
    public async Task WorkspaceDelete_WhenWorkspaceIsUserWorkspace_ShouldReturnUserWorkspaceError()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces[2];

        // Act
        Client.UseCustomToken(WorkspaceData.Users[0]);
        var outcome = await Client.Delete($"api/workspaces/{workspace.Id}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.UserWorkspace);
    }

    [Fact]
    public async Task WorkspaceDelete_WhenWorkspaceIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var outcome = await Client.Delete($"api/workspaces/{id}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(WorkspaceData.Workspaces.Length);
        dbContext.Workspaces.AsEnumerable()
            .SingleOrDefault(w => w.Id.Value == id)
            .Should().BeNull();
    }
}