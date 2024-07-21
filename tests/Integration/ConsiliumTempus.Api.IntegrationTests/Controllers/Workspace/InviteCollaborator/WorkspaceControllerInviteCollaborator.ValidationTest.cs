using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.InviteCollaborator;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerInviteCollaboratorValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task InviteCollaboratorToWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest(id: workspace.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Post("api/workspaces/invite-collaborator", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task InviteCollaboratorToWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateInviteCollaboratorToWorkspaceRequest(
            id: Guid.Empty, 
            email: string.Empty);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Post("api/workspaces/invite-collaborator", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}