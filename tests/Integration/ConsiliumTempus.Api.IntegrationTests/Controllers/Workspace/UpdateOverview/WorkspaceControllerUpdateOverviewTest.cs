using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.UpdateOverview;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.UpdateOverview;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateOverviewOverviewTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateOverviewWorkspace_WhenItSucceeds_ShouldUpdateOverviewAndReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateOverviewWorkspaceRequest(id: workspace.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Put("api/workspaces/overview", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateOverviewWorkspaceResponse>();
        response!.Message.Should().Be("Workspace Overview has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedWorkspace = dbContext.Workspaces
            .Single(u => u.Id == WorkspaceId.Create(request.Id));
        Utils.Workspace.AssertUpdatedOverview(updatedWorkspace!, request);
    }

    [Fact]
    public async Task UpdateOverviewWorkspace_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateOverviewWorkspaceRequest(id: Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/workspaces/overview", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.SingleOrDefault(u => u.Id == WorkspaceId.Create(request.Id))
            .Should().BeNull();
    }
}