using System.Net;
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

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerUpdateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceUpdateWithAdminRole_ShouldUpdateAndReturnNewWorkspace()
    {
        await AssertSuccessfulRequest(GetUpdateRequest(), "michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithMemberRole_ShouldReturnNewWorkspace()
    {
        await AssertSuccessfulRequest(GetUpdateRequest(), "stephenc@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest(GetUpdateRequest(), "lebronj@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest(GetUpdateRequest(), "leom@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = GetUpdateRequest("90000000-0000-0000-0000-000000000000");
        
        // Act
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        (await GetWorkspaceById(request.Id)).Should().BeNull();
        
        await outcome.ValidateError(Errors.Workspace.NotFound);
    }

    private async Task AssertSuccessfulRequest(UpdateWorkspaceRequest request, string email)
    {
        // Arrange - parameters
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        var updatedWorkspace = await GetWorkspaceById(request.Id);
        Utils.Workspace.AssertUpdated(updatedWorkspace!, request);
        
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            request.Name!, 
            request.Description!);
    }

    private async Task AssertForbiddenRequest(UpdateWorkspaceRequest request, string email)
    {
        // Arrange - parameters
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        var updatedWorkspace = await GetWorkspaceById(request.Id);
        Utils.Workspace.AssertNotUpdated(updatedWorkspace!, request);
        
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private static UpdateWorkspaceRequest GetUpdateRequest(string id = "10000000-0000-0000-0000-000000000000")
    {
        return new UpdateWorkspaceRequest(
            new Guid(id),
            "Workspace New Name",
            "This is a new description");
    }
    
    private async Task<WorkspaceAggregate?> GetWorkspaceById(Guid id)
    {
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        return await dbContext.Workspaces.SingleOrDefaultAsync(u => u.Id == WorkspaceId.Create(id));
    }
}