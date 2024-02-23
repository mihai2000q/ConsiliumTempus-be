using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Create;

public class WorkspaceControllerCreateValidationTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WorkspaceCreate_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/workspaces", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WorkspaceCreate_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest(
            name: string.Empty,
            description: new string('a', 2000));
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/workspaces", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}