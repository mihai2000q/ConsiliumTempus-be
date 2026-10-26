using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.AcceptInvitation;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Common.Entities;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Entities;
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
            .Include(w => w.Invitations)
            .Include(w => w.Memberships)
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
    public async Task AcceptInvitationToWorkspace_WhenUserIsNotTheInvitedCollaborator_ShouldReturnNotInvitedUserError()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(
            invitation.Workspace.Id.Value,
            invitation.Id.Value);

        // Act
        Client.UseCustomToken(invitation.Sender);
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        await outcome.ValidateError(Errors.WorkspaceInvitation.NotInvitedUser);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<WorkspaceInvitation>()
            .SingleOrDefault(i => i.Id == invitation.Id)
            .Should().NotBeNull();
        dbContext.Workspaces
            .Include(w => w.Memberships)
            .Single(w => w.Id == invitation.Workspace.Id)
            .Memberships.Should().NotContain(m => m.User.Id == invitation.Collaborator.Id);
    }

    [Fact]
    public async Task AcceptInvitationToWorkspace_WhenUserIsAlreadyCollaborator_ShouldReturnAlreadyCollaboratorError()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var collaborator = invitation.Collaborator;
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(
            invitation.Workspace.Id.Value,
            invitation.Id.Value);

        await using (var arrangeDbContext = await DbContextFactory.CreateDbContextAsync())
        {
            arrangeDbContext.Set<Membership>().Add(MembershipFactory.Create(
                arrangeDbContext.Users.Single(u => u.Id == collaborator.Id),
                arrangeDbContext.Workspaces.Single(w => w.Id == invitation.Workspace.Id),
                WorkspaceRole.View));
            await arrangeDbContext.SaveChangesAsync();
        }

        // Act
        Client.UseCustomToken(collaborator);
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        await outcome.ValidateError(Errors.WorkspaceInvitation.AlreadyCollaborator);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<WorkspaceInvitation>()
            .SingleOrDefault(i => i.Id == invitation.Id)
            .Should().NotBeNull();
        dbContext.Set<Membership>()
            .Where(m => m.Workspace.Id == invitation.Workspace.Id)
            .Count(m => m.User.Id == collaborator.Id)
            .Should().Be(1);
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