using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetCollection;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetCollectionValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetCollectionWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest(order: "name.asc");

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces?order={request.Order}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest(
            order: "something wrong",
            pageSize: -1,
            currentPage: 0);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces?order={request.Order}" +
                                       $"&pageSize={request.PageSize}" +
                                       $"&currentPage={request.CurrentPage}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}