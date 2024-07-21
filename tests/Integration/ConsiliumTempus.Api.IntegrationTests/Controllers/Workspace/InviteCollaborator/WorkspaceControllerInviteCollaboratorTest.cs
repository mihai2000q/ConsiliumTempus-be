using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.InviteCollaborator;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.InviteCollaborator;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerInviteCollaboratorTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task InviteCollaboratorToWorkspace_WhenItSucceeds_ShouldSendInvitationAndReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces[0];
        var collaborator = WorkspaceData.Users[6];
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest(
            workspace.Id.Value,
            collaborator.Credentials.Email);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/workspaces/invite-collaborator", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<InviteCollaboratorToWorkspaceResponse>();
        response!.Message.Should().Be("Collaborator has been successfully invited to workspace!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<WorkspaceInvitation>().Should().HaveCount(WorkspaceData.WorkspaceInvitations.Length + 1);
        var updatedWorkspace = dbContext.Workspaces
            .AsNoTracking()
            .Include(w => w.Invitations)
            .Single(w => w.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertInviteCollaborator(request, updatedWorkspace, user, collaborator);
    }

    [Fact]
    public async Task InviteCollaboratorToWorkspace_WhenUserIsAlreadyCollaborator_ShouldReturnAlreadyCollaboratorError()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces[0];
        var collaborator = WorkspaceData.Users[3];
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest(
            workspace.Id.Value,
            collaborator.Credentials.Email);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/workspaces/invite-collaborator", request);

        // Assert
        await outcome.ValidateError(Errors.WorkspaceInvitation.AlreadyCollaborator);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync(); 
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().NotBeNull();
        dbContext
            .Set<Membership>()
            .Where(m => m.Workspace.Id == WorkspaceId.Create(request.Id))
            .SingleOrDefault(m => m.User.Credentials.Email == request.Email)
            .Should().NotBeNull();
    }

    [Fact]
    public async Task InviteCollaboratorToWorkspace_WhenUserIsAlreadyInvited_ShouldReturnAlreadyInvitedError()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces[0];
        var collaborator = WorkspaceData.Users[7];
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest(
            workspace.Id.Value,
            collaborator.Credentials.Email);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/workspaces/invite-collaborator", request);

        // Assert
        await outcome.ValidateError(Errors.WorkspaceInvitation.AlreadyInvited);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync(); 
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().NotBeNull();
        dbContext
            .Set<WorkspaceInvitation>()
            .Where(wi => wi.Workspace.Id == WorkspaceId.Create(request.Id))
            .SingleOrDefault(wi => wi.Collaborator.Credentials.Email == request.Email)
            .Should().NotBeNull();
    }

    [Fact]
    public async Task InviteCollaboratorToWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/workspaces/invite-collaborator", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync(); 
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}