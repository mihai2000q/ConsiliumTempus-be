using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Delete;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerDeleteValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task DeleteWorkspace_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateDeleteWorkspaceRequest(
            WorkspaceData.Workspaces.First().Id.Value);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Delete($"api/workspaces/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateDeleteWorkspaceRequest(
            id: Guid.Empty);

        // Act
        var outcome = await Client.Delete($"api/workspaces/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}