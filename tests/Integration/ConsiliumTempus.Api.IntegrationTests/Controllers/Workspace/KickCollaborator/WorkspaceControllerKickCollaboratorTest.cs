using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.KickCollaborator;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.KickCollaborator;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerKickCollaboratorTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task KickCollaboratorFromWorkspace_WhenItSucceeds_ShouldKickCollaboratorAndReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var workspace = WorkspaceData.Workspaces.First();
        var collaborator = workspace.Memberships
            .First(m => m.User != user && m.User != workspace.Owner)
            .User;
        var request = WorkspaceRequestFactory.CreateKickCollaboratorFromWorkspaceRequest(
            id: workspace.Id.Value,
            collaboratorId: collaborator.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/workspaces/" +
                                          $"{request.Id}/Kick-Collaborator/{request.CollaboratorId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<KickCollaboratorFromWorkspaceResponse>();
        response!.Message.Should().Be("Collaborator has been kicked from workspace successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedWorkspace = dbContext.Workspaces
            .Include(w => w.Memberships)
            .Single(u => u.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertKickCollaborator(request, updatedWorkspace);
    }

    [Fact]
    public async Task KickCollaboratorFromWorkspace_WhenCurrentUserIsKicked_ShouldReturnKickYourselfError()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var user = workspace.Memberships
            .First(m => m.User != workspace.Owner && m.WorkspaceRole == WorkspaceRole.Admin)
            .User;
        var request = WorkspaceRequestFactory.CreateKickCollaboratorFromWorkspaceRequest(
            workspace.Id.Value,
            user.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/workspaces/" +
                                          $"{request.Id}/Kick-Collaborator/{request.CollaboratorId}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.KickYourself);
    }

    [Fact]
    public async Task KickCollaboratorFromWorkspace_WhenOwnerIsKicked_ShouldReturnKickOwnerError()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var user = workspace.Memberships
            .First(m => m.User != workspace.Owner && m.WorkspaceRole == WorkspaceRole.Admin)
            .User;
        var request = WorkspaceRequestFactory.CreateKickCollaboratorFromWorkspaceRequest(
            workspace.Id.Value,
            workspace.Owner.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/workspaces/" +
                                          $"{request.Id}/Kick-Collaborator/{request.CollaboratorId}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.KickOwner);
    }

    [Fact]
    public async Task KickCollaboratorFromWorkspace_WhenCollaboratorIsNotFound_ShouldReturnCollaboratorNotFoundError()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateKickCollaboratorFromWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Delete($"api/workspaces/" +
                                          $"{request.Id}/Kick-Collaborator/{request.CollaboratorId}");

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
    public async Task KickCollaboratorFromWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateKickCollaboratorFromWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Delete($"api/workspaces/" +
                                          $"{request.Id}/Kick-Collaborator/{request.CollaboratorId}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.SingleOrDefault(u => u.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}