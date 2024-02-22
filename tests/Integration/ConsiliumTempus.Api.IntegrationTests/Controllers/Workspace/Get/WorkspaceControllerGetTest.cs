using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Get;

public class WorkspaceControllerGetTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WorkspaceGet_WhenItSucceeds_ShouldReturnDto()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";

        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.GetAsync($"api/workspaces/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome,
            "Basketball",
            "This is the Description of the first Workspace");
    }

    [Fact]
    public async Task WorkspaceUpdate_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "50000000-0000-0000-0000-000000000000";

        // Act
        var outcome = await Client.GetAsync($"api/workspaces/{id}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
    }
}