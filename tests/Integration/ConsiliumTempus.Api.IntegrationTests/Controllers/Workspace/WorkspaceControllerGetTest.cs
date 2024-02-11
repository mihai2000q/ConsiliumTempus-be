using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerGetTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceGetWithAdminRole_ShouldReturnWorkspace()
    {
        await AssertSuccessfulResponse("michaelj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceGetWithMemberRole_ShouldReturnWorkspace()
    {
        await AssertSuccessfulResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceGetWithViewRole_ShouldReturnWorkspace()
    {
        await AssertSuccessfulResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceGetWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceGetFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";

        // Act
        var outcome = await Client.GetAsync($"api/workspaces/{id}");

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, "Workspace could not be found");
    }

    private async Task AssertSuccessfulResponse(string email, string id = "10000000-0000-0000-0000-000000000000")
    {
        // Arrange - parameters

        // Act
        UseCustomToken(email);
        var outcome = await Client.GetAsync($"api/workspaces/{id}");

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome,
            "Workspace 1",
            "This is the Description of the first Workspace");
    }

    private async Task AssertForbiddenResponse(string email, string id = "10000000-0000-0000-0000-000000000000")
    {
        // Arrange - parameters

        // Act
        UseCustomToken(email);
        var outcome = await Client.GetAsync($"api/workspaces/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}