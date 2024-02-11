using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceDeleteWithAdminRole_ShouldReturnWorkspace()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "50000000-0000-0000-0000-000000000000";

        // Act
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, "Workspace could not be found");
    }

    private async Task AssertSuccessfulRequest(string email, string id = "10000000-0000-0000-0000-000000000000")
    {
        // Arrange - parameters

        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome,
            "Basketball",
            "This is the Description of the first Workspace");
    }

    private async Task AssertForbiddenResponse(string email, string id = "10000000-0000-0000-0000-000000000000")
    {
        // Arrange - parameters

        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}