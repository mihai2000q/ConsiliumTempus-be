using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
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
        var response = await outcome.Content.ReadFromJsonAsync<CreateWorkspaceResponse>();
        response!.Message.Should().Be("Workspace has been created successfully!");
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(WorkspaceData.Workspaces.Length + 1);
        var createdWorkspace = await dbContext.Workspaces
            .SingleAsync(w => w.Name.Value == request.Name);
        Utils.Workspace.AssertCreation(createdWorkspace, request);
    }
    
    [Fact]
    public async Task WhenWorkspaceCreateFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateCreateWorkspaceRequest();
        
        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Post("api/workspaces", request);

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);

        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(WorkspaceData.Workspaces.Length);
    }
}