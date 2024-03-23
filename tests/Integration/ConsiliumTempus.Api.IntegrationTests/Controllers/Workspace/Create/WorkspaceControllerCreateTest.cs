using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Create;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerCreateTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldCreateAndReturnNewWorkspace()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();
        
        // Act
        var outcome = await Client.Post("api/workspaces", request);

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(1);
        var createdWorkspace = await dbContext.Workspaces.FirstAsync();
        Utils.Workspace.AssertCreation(createdWorkspace, request);
        
        await Utils.Workspace.AssertDtoFromResponse(outcome, request.Name, request.Description);
    }
}