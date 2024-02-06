using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;

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
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<WorkspaceDto>();
        response?.Id.Should().NotBeNullOrWhiteSpace();
        response?.Name.Should().Be(request.Name);
        response?.Description.Should().Be(request.Description);
    }
    
    [Fact]
    public async Task WhenWorkspaceCreateFails_ShouldReturnInvalidTokenError()
    {
        // Arrange
        var request = new CreateWorkspaceRequest(
            "My Workspace",
            "This is your workspace where you can place projects");
        
        // Act
        UseInvalidToken();
        var outcome = await Client.PostAsJsonAsync("api/workspaces/Create", request);

        // Assert
        await outcome.ValidateError(HttpStatusCode.Unauthorized, "Invalid Token");
    }
}