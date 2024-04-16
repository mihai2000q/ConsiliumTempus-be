using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Delete;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerDeleteAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task DeleteWorkspace_WhenWithAdminRole_ShouldDeleteAndReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(WorkspaceData.Users[0]);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(WorkspaceData.Users[3]);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(WorkspaceData.Users[4]);
    }

    [Fact]
    public async Task DeleteWorkspace_WhenWithoutMembership_ShouldReturnForbiddenResponse()
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

        // Act
        Client.UseCustomToken(user);
        return await Client.Delete($"api/workspaces/{workspace.Id}");
    }
}