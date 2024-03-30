using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Create;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerCreateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task WhenWorkspaceCreateIsSuccessful_ShouldCreateAndReturnNewWorkspace()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();
        
        // Act
        var outcome = await Client.Post("api/workspaces", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(WorkspaceData.Workspaces.Length + 1);
        var createdWorkspace = await dbContext.Workspaces
            .SingleAsync(w => w.Name.Value == request.Name);
        Utils.Workspace.AssertCreation(createdWorkspace, request);
        
        await Utils.Workspace.AssertDtoFromResponse(outcome, createdWorkspace);
    }
}