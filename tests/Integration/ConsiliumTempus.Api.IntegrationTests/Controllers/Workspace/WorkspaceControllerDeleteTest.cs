using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceDeleteWithAdminRole_ShouldReturnWorkspace()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.DeleteAsync($"api/workspace/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            "Workspace 1", 
            "This is the Description of the first Workspace");
    }
    
    [Fact]
    public async Task WhenWorkspaceDeleteWithMemberRole_ShouldReturnForbiddenResponse()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.DeleteAsync($"api/workspace/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task WhenWorkspaceDeleteWithViewRole_ShouldReturnForbiddenResponse()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("lebronj@gmail.com");
        var outcome = await Client.DeleteAsync($"api/workspace/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task WhenWorkspaceDeleteWithoutMembership_ShouldReturnForbiddenResponse()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("leom@gmail.com");
        var outcome = await Client.DeleteAsync($"api/workspace/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task WhenWorkspaceDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/workspace/{id}");

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, "Workspace could not be found");
    }
}