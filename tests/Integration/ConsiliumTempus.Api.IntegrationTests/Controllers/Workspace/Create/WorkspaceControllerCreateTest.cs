using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Create;

public class WorkspaceControllerCreateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldCreateAndReturnNewWorkspace()
    {
        // Arrange
        var request = new CreateWorkspaceRequest(
            "My Workspace",
            "This is your workspace where you can place projects");
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/workspaces", request);

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(1);
        var createdWorkspace = await dbContext.Workspaces.FirstAsync();
        Utils.Workspace.AssertCreation(createdWorkspace, request);
        
        await Utils.Workspace.AssertDtoFromResponse(outcome, request.Name, request.Description);
    }
}