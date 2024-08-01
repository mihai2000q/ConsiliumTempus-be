using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.KickCollaborator;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerKickCollaboratorValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task KickCollaboratorFromWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
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
    }

    [Fact]
    public async Task KickCollaboratorFromWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateKickCollaboratorFromWorkspaceRequest(
            id: Guid.Empty);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Delete($"api/workspaces/" +
                                          $"{request.Id}/Kick-Collaborator/{request.CollaboratorId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}