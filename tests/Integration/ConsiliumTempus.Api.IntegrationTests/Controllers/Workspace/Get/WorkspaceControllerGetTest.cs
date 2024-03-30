using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Get;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task WorkspaceGet_WhenItSucceeds_ShouldReturnDto()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();

        // Act
        Client.UseCustomToken(WorkspaceData.Users[0]);
        var outcome = await Client.Get($"api/workspaces/{workspace.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await Utils.Workspace.AssertDtoFromResponse(outcome, workspace);
    }

    [Fact]
    public async Task WorkspaceUpdate_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var outcome = await Client.Get($"api/workspaces/{id}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
    }
}