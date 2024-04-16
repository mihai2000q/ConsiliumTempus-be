using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Update;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateWorkspace_WhenItSucceeds_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest(id: workspace.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Put("api/workspaces", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateWorkspaceResponse>();
        response!.Message.Should().Be("Workspace has been updated successfully!");

        var updatedWorkspace = await GetWorkspaceById(request.Id);
        Utils.Workspace.AssertUpdated(workspace, updatedWorkspace!, request);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/workspaces", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        (await GetWorkspaceById(request.Id)).Should().BeNull();
    }

    private async Task<WorkspaceAggregate?> GetWorkspaceById(Guid id)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        return await dbContext.Workspaces.SingleOrDefaultAsync(u => u.Id == WorkspaceId.Create(id));
    }
}