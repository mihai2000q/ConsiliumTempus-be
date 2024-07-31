using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.UpdateCollaborator;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateCollaboratorValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateCollaboratorFromWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var workspace = WorkspaceData.Workspaces.First();
        var collaborator = workspace.Memberships.First(m => m.User != user).User;
        var request = WorkspaceRequestFactory.CreateUpdateCollaboratorFromWorkspaceRequest(
            id: workspace.Id.Value,
            collaboratorId: collaborator.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/workspaces/Collaborator", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCollaboratorFromWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateCollaboratorFromWorkspaceRequest(
            id: Guid.Empty);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Put("api/workspaces/Collaborator", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}