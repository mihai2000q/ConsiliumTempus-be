using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Create;

public class ProjectControllerCreateAuthorizationTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Project")
{
    [Fact]
    public async Task WhenProjectCreateWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }

    [Fact]
    public async Task WhenProjectCreateWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenProjectCreateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenProjectCreateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    private async Task AssertSuccessfulRequest(string email)
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
        var request = new CreateProjectRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "Project Name",
            "This is the project description",
            true);

        // Act
        UseCustomToken(email);
        return await Client.PostAsJsonAsync("api/projects", request);
    }
}