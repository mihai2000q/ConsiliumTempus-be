using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.GetOverview;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetOverview;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetOverviewOverviewTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetOverview_WhenItSucceeds_ShouldReturnWorkspaceOverview()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces.First();
        var query = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/workspaces/overview/{query.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetOverviewWorkspaceResponse>();
        Utils.Workspace.AssertGetOverviewResponse(response!, workspace);
    }

    [Fact]
    public async Task GetOverview_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = WorkspaceRequestFactory.CreateGetOverviewWorkspaceRequest();

        // Act
        var outcome = await Client.Get($"api/workspaces/overview/{query.Id}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
    }
}