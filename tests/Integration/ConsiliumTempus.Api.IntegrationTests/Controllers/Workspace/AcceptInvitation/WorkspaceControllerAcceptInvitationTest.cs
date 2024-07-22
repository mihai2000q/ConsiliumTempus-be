using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.AcceptInvitation;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerAcceptInvitationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task AcceptInvitationToWorkspace_WhenItSucceeds_ShouldSendInvitationAndReturnSuccessResponse()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var collaborator = invitation.Collaborator;
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(
            invitation.Workspace.Id.Value,
            invitation.Id.Value);

        // Act
        Client.UseCustomToken(collaborator);
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<AcceptInvitationToWorkspaceResponse>();
        response!.Message.Should().Be("Invitation to Workspace has been successfully accepted!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<WorkspaceInvitation>().Should().HaveCount(WorkspaceData.WorkspaceInvitations.Length - 1);
        dbContext.Set<WorkspaceInvitation>()
            .AsNoTracking()
            .SingleOrDefault(i => i.Id == invitation.Id)
            .Should().BeNull();

        var updatedWorkspace = dbContext.Workspaces
            .AsNoTracking()
            .Include(w => w.Memberships)
            .ThenInclude(m => m.User)
            .Single(w => w.Id == invitation.Workspace.Id);
        Utils.Workspace.AssertAcceptInvitation(request, updatedWorkspace, collaborator);
    }

    [Fact]
    public async Task AcceptInvitationToWorkspace_WhenUserIsNotFound_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(
            id: invitation.Workspace.Id.Value,
            invitationId: invitation.Id.Value);

        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync(); 
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Set<WorkspaceInvitation>()
            .SingleOrDefault(i => i.Id == WorkspaceInvitationId.Create(request.InvitationId))
            .Should().NotBeNull();
    }

    [Fact]
    public async Task AcceptInvitationToWorkspace_WhenInvitationIsNotFound_ShouldReturnInvitationNotFoundError()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(
            id: invitation.Workspace.Id.Value);

        // Act
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        await outcome.ValidateError(Errors.WorkspaceInvitation.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync(); 
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Set<WorkspaceInvitation>()
            .SingleOrDefault(i => i.Id == WorkspaceInvitationId.Create(request.Id))
            .Should().BeNull();
    }

    [Fact]
    public async Task AcceptInvitationToWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync(); 
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}