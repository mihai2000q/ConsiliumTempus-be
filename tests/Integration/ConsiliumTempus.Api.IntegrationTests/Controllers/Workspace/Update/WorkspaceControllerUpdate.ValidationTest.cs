using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Update;

public class WorkspaceControllerUpdateValidationTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WorkspaceUpdate_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new UpdateWorkspaceRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "Workspace New Name",
            "This is a new description");
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WorkspaceUpdate_WhenCommandIsInvalid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new UpdateWorkspaceRequest(
            Guid.Empty, 
            string.Empty,
            "This is a new description");
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}