using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Update;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateAuthorizationTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task WhenWorkspaceUpdateWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(WorkspaceData.Users[0]);
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(WorkspaceData.Users[3]);
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest(WorkspaceData.Users[4]);
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest(WorkspaceData.Users[1]);
    }
    private async Task AssertSuccessfulRequest(UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenRequest(UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user)
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest(id: workspace.Id.Value);
        
        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/workspaces", request);
    }
}