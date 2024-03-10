using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Update;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateAuthorizationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceUpdateWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest("stephenc@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest("lebronj@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest("leom@gmail.com");
    }
    private async Task AssertSuccessfulRequest(string email)
    {
        var outcome = await ArrangeAndAct(email);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenRequest(string email)
    {
        var outcome = await ArrangeAndAct(email);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(string email)
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest(
            id: new Guid("10000000-0000-0000-0000-000000000000"));
        
        // Act
        Client.UseCustomToken(email);
        return await Client.Put("api/workspaces", request);
    }
}