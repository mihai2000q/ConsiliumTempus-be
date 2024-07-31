using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Leave;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerLeaveValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task LeaveWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users[3];
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/workspaces/{request.Id}/leave");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task LeaveWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest(id: Guid.Empty);

        // Act
        var outcome = await Client.Delete($"api/workspaces/{request.Id}/leave");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}