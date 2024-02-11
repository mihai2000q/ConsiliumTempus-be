using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerGetTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceGetWithAdminRole_ShouldReturnWorkspace()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.GetAsync($"api/workspace/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            "Workspace 1", 
            "This is the Description of the first Workspace");
    }
    
    [Fact]
    public async Task WhenWorkspaceGetWithMemberRole_ShouldReturnWorkspace()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.GetAsync($"api/workspace/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            "Workspace 1", 
            "This is the Description of the first Workspace");
    }
    
    [Fact]
    public async Task WhenWorkspaceGetWithViewRole_ShouldReturnWorkspace()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("lebronj@gmail.com");
        var outcome = await Client.GetAsync($"api/workspace/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            "Workspace 1", 
            "This is the Description of the first Workspace");
    }
    
    [Fact]
    public async Task WhenWorkspaceGetWithoutMembership_ShouldReturnForbiddenResponse()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("leom@gmail.com");
        var outcome = await Client.GetAsync($"api/workspace/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task WhenWorkspaceGetFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.GetAsync($"api/workspace/{id}");

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, "Workspace could not be found");
    }
}