using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.RejectInvitation;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.Entities;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.RejectInvitation;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerRejectInvitationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task RejectInvitationToWorkspace_WhenItSucceeds_ShouldSendInvitationAndReturnSuccessResponse()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var collaborator = invitation.Collaborator;
        var request = WorkspaceRequestFactory.CreateRejectInvitationToWorkspaceRequest(
            invitation.Workspace.Id.Value,
            invitation.Id.Value);

        // Act
        Client.UseCustomToken(collaborator);
        var outcome = await Client.Post("api/workspaces/reject-invitation", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<RejectInvitationToWorkspaceResponse>();
        response!.Message.Should().Be("Invitation to Workspace has been successfully rejected!");

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
        Utils.Workspace.AssertRejectInvitation(request, updatedWorkspace, collaborator);
    }

    [Fact]
    public async Task RejectInvitationToWorkspace_WhenInvitationIsNotFound_ShouldReturnInvitationNotFoundError()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var request = WorkspaceRequestFactory.CreateRejectInvitationToWorkspaceRequest(
            id: invitation.Workspace.Id.Value);

        // Act
        var outcome = await Client.Post("api/workspaces/reject-invitation", request);

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
    public async Task RejectInvitationToWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateRejectInvitationToWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/workspaces/reject-invitation", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync(); 
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}