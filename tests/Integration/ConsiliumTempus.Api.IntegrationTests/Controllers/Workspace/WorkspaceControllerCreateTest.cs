using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerCreateTest(ConsiliumTempusWebApplicationFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldReturnNewWorkspace()
    {
        // Arrange
        var request = new CreateWorkspaceRequest(
            "My Workspace",
            "This is your workspace where you can place projects");
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/workspaces/Create", request);

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(outcome, request.Name, request.Description);
    }
}