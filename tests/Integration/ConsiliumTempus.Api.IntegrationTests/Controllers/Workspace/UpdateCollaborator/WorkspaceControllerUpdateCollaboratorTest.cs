using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateCollaborator;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.UpdateCollaborator;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateCollaboratorTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateCollaboratorFromWorkspace_WhenItSucceeds_ShouldUpdateCollaboratorAndReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var workspace = WorkspaceData.Workspaces.First();
        var collaborator = workspace.Memberships.First(m => m.User != user).User;
        var request = WorkspaceRequestFactory.CreateUpdateCollaboratorFromWorkspaceRequest(
            workspace.Id.Value,
            collaborator.Id.Value,
            nameof(WorkspaceRole.View));

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/workspaces/Collaborators", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateCollaboratorFromWorkspaceResponse>();
        response!.Message.Should().Be("Collaborator has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedWorkspace = dbContext.Workspaces
            .Include(w => w.Memberships)
            .Single(u => u.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertUpdateCollaborator(updatedWorkspace, request);
    }

    [Fact]
    public async Task UpdateCollaboratorFromWorkspace_WhenCollaboratorIsNotFound_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateCollaboratorFromWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Put("api/workspaces/Collaborators", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.CollaboratorNotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces
            .Include(w => w.Memberships)
            .Single(u => u.Id == WorkspaceId.Create(request.Id))
            .Memberships.SingleOrDefault(m => m.User.Id.Value == request.CollaboratorId)
            .Should().BeNull();
    }

    [Fact]
    public async Task UpdateCollaboratorFromWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateCollaboratorFromWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/workspaces/Collaborators", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.SingleOrDefault(u => u.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}