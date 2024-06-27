using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetOverview;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetOverviewValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetOverview_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest(id: workspace.Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces/overview/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOverview_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest(
            id: Guid.Empty);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces/overview/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}