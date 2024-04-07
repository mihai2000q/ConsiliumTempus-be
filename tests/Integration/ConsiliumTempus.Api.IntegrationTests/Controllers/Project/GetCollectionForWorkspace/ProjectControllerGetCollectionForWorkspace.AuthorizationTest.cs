using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollectionForWorkspace;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionForWorkspaceAuthorizationTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task WhenProjectSprintCreateWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(ProjectData.Users[0]);
    }
    
    [Fact]
    public async Task WhenProjectSprintCreateWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertSuccessfulRequest(ProjectData.Users[3]);
    }

    [Fact]
    public async Task WhenProjectSprintCreateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertSuccessfulRequest(ProjectData.Users[4]);
    }

    [Fact]
    public async Task WhenProjectSprintCreateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[1]);
    }

    private async Task AssertSuccessfulRequest(UserAggregate user)
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
        var request = ProjectRequestFactory.CreateGetCollectionProjectForWorkspaceRequest(
            ProjectData.Workspaces[0].Id.Value);
        
        // Act
        Client.UseCustomToken(user);
        return await Client.Get($"api/projects/workspace?workspaceId={request.WorkspaceId}");
    }
}