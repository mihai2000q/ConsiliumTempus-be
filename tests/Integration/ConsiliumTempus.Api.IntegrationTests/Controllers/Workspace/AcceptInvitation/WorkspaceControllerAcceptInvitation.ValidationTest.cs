using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.AcceptInvitation;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerAcceptInvitationValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task AcceptInvitationToWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var invitation = WorkspaceData.WorkspaceInvitations.First();
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(
            id: invitation.Workspace.Id.Value,
            invitationId: invitation.Id.Value);

        // Act
        Client.UseCustomToken(invitation.Collaborator);
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AcceptInvitationToWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateAcceptInvitationToWorkspaceRequest(
            id: Guid.Empty, 
            invitationId: Guid.Empty);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Post("api/workspaces/accept-invitation", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}