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
    public async Task UpdateWorkspace_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(WorkspaceData.Users[0]);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(WorkspaceData.Users[3]);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(WorkspaceData.Users[4]);
    }

    [Fact]
    public async Task UpdateWorkspace_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(WorkspaceData.Users[1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user)
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