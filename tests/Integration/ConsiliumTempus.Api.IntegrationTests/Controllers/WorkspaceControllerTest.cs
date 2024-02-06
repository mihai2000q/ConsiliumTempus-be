using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using FluentAssertions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers;

public class WorkspaceControllerTest(ConsiliumTempusWebApplicationFactory factory)
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
        var outcome = await Client.PostAsJsonAsync("api/workspace/Create", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateWorkspaceResponse>();
        response?.Id.Should().NotBeNullOrWhiteSpace();
        response?.Name.Should().Be(request.Name);
        response?.Description.Should().Be(request.Description);
    }
}