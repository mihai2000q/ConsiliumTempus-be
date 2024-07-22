using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.RejectInvitation;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerRejectInvitationValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task RejectInvitationToWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var request = WorkspaceRequestFactory.CreateRejectInvitationToWorkspaceRequest(
            id: invitation.Workspace.Id.Value,
            invitationId: invitation.Id.Value);

        // Act
        Client.UseCustomToken(invitation.Collaborator);
        var outcome = await Client.Post("api/workspaces/Reject-invitation", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RejectInvitationToWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateRejectInvitationToWorkspaceRequest(
            id: Guid.Empty, 
            invitationId: Guid.Empty);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Post("api/workspaces/Reject-invitation", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}