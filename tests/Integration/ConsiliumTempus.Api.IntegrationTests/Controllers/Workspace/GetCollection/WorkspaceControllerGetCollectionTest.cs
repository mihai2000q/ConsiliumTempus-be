using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetCollection;

public class WorkspaceControllerGetCollectionTest(
    ConsiliumTempusWebApplicationFactory factory, 
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceGetCollectionForUser_ShouldReturnAllTheWorkspacesForUser()
    {
        // Arrange
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.GetAsync("api/Workspaces");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await outcome.Content.ReadFromJsonAsync<List<WorkspaceDto>>();
        content.Should().HaveCount(2);
        Utils.Workspace.AssertDto(
            content![0],
            "Basketball",
            "This is the Description of the first Workspace",
            "10000000-0000-0000-0000-000000000000");
        
        Utils.Workspace.AssertDto(
            content[1],
            "Michael Group",
            "This is the Description of the third Workspace",
            "30000000-0000-0000-0000-000000000000");
    }
}