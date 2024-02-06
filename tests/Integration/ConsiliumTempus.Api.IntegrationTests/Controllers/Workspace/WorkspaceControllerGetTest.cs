using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerGetTest(ConsiliumTempusWebApplicationFactory factory) 
    : BaseIntegrationTest(factory, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceGetIsSuccessful_ShouldReturnWorkspace()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.GetAsync($"api/workspaces/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            "Workspace 1", 
            "This is the Description of the first Workspace");
    }
    
    [Fact]
    public async Task WhenWorkspaceGetFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.GetAsync($"api/workspaces/{id}");

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, "Workspace could not be found");
    }
}