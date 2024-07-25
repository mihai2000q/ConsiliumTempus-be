using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Leave;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Leave;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerLeaveTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task LeaveWorkspace_WhenIsSuccessful_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var user = WorkspaceData.Users[3];
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/workspaces/Leave", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<LeaveWorkspaceResponse>();
        response!.Message.Should().Be("Workspace has been left successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedWorkspace = await dbContext.Workspaces
            .Include(w => w.Memberships)
            .ThenInclude(m => m.User)
            .SingleAsync(w => w.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertLeave(request, updatedWorkspace, user);
    }

    [Fact]
    public async Task LeaveWorkspace_WhenUserIsOwner_ShouldReturnLeaveOwnedWorkspaceError()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/workspaces/Leave", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.LeaveOwnedWorkspace);

        workspace.Owner.Should().Be(user);
    }
    
    [Fact]
    public async Task LeaveWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateLeaveWorkspaceRequest();

        // Act
        var outcome = await Client.Post("api/workspaces/Leave", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}