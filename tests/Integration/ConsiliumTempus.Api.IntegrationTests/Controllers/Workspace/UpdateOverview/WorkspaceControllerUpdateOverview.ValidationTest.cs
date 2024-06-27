using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.UpdateOverview;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateOverviewValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task UpdateOverviewWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateOverviewWorkspaceRequest(id: workspace.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Put("api/workspaces/overview", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateOverviewWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateOverviewWorkspaceRequest(
            id: Guid.Empty);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Put("api/workspaces/overview", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}