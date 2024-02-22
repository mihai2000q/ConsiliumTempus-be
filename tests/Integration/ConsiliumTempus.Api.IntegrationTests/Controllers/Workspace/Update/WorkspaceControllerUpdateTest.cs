using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Update;

public class WorkspaceControllerUpdateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WorkspaceUpdate_WhenItSucceeds_ShouldUpdateAndReturnNewWorkspace()
    {
        // Arrange
        var request = new UpdateWorkspaceRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "Workspace New Name",
            "This is a new description");
        
        // Act
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            request.Name, 
            request.Description);
        
        var updatedWorkspace = await GetWorkspaceById(request.Id);
        Utils.Workspace.AssertUpdated(updatedWorkspace!, request);
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = new UpdateWorkspaceRequest(
            new Guid("90000000-0000-0000-0000-000000000000"),
            "Workspace New Name",
            "This is a new description");
        
        // Act
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
        
        (await GetWorkspaceById(request.Id)).Should().BeNull();
    }
    
    private async Task<WorkspaceAggregate?> GetWorkspaceById(Guid id)
    {
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        return await dbContext.Workspaces.SingleOrDefaultAsync(u => u.Id == WorkspaceId.Create(id));
    }
}