using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Delete;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerDeleteAuthorizationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Project")
{
    [Fact]
    public async Task WhenProjectDeleteWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenProjectDeleteWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenProjectDeleteWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenProjectDeleteWithoutMembership_ShouldReturnForbiddenResponse()
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
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        Client.UseCustomToken(email);
        return await Client.Delete($"api/projects/{id}");
    }
}