using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOwner;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.UpdateOwner;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateOwnerOwnerTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateOwnerWorkspace_WhenItSucceeds_ShouldUpdateOwnerAndReturnSuccessResponse()
    {
        // Arrange
        var owner = WorkspaceData.Users[3];
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateOwnerWorkspaceRequest(
            workspace.Id.Value,
            owner.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Put("api/workspaces/Owner", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateOwnerWorkspaceResponse>();
        response!.Message.Should().Be("Workspace owner has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedWorkspace = dbContext.Workspaces
            .AsNoTracking()
            .Include(w => w.Owner)
            .Single(u => u.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertUpdatedOwner(updatedWorkspace, request, owner);
    }

    [Fact]
    public async Task UpdateOwnerWorkspace_WhenCollaboratorIsNotFound_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var user = WorkspaceData.Users[1]; // not part of the workspace, therefore, not a collaborator
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateOwnerWorkspaceRequest(
            id: workspace.Id.Value,
            ownerId: user.Id.Value);

        // Act
        var outcome = await Client.Put("api/workspaces/Owner", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.CollaboratorNotFound);
    }
    
    [Fact]
    public async Task UpdateOwnerWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateOwnerWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/workspaces/Owner", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.SingleOrDefault(u => u.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}