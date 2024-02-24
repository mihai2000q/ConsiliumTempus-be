using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Get;

public class WorkspaceControllerGetAuthorizationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceGetWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse("michaelj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceGetWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceGetWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceGetWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    private async Task AssertSuccessfulResponse(string email)
    {
        var outcome = await ArrangeAndAct(email);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(string email)
    {
        var outcome = await ArrangeAndAct(email);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(string email)
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";

        // Act
        UseCustomToken(email);
        return await Client.GetAsync($"api/workspaces/{id}");
    }
}