using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Get;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetWorkspace_WhenItSucceeds_ShouldReturnWorkspace()
    {
        // Arrange
        var user = WorkspaceData.Users[0];
        var workspace = WorkspaceData.Workspaces.First();
        var query = WorkspaceRequestFactory.CreateGetWorkspaceRequest(workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/workspaces/{query.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetWorkspaceResponse>();
        Utils.Workspace.AssertGetResponse(response!, workspace, user);
    }

    [Fact]
    public async Task GetWorkspace_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var query = WorkspaceRequestFactory.CreateGetWorkspaceRequest();

        // Act
        var outcome = await Client.Get($"api/workspaces/{query.Id}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
    }
}