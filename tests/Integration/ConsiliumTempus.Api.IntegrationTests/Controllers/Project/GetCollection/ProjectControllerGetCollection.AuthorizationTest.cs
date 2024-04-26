using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollection;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetCollectionProject_WithoutWorkspace_ShouldReturnSuccessResponse()
    {
        // Arrange

        // Act
        Client.UseCustomToken(ProjectData.Users[0]);
        var outcome = await Client.Get("api/projects");
        
        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetCollectionProject_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[0]);
    }

    [Fact]
    public async Task GetCollectionProject_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[3]);
    }

    [Fact]
    public async Task GetCollectionProject_WhenWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[4]);
    }

    [Fact]
    public async Task GetCollectionProject_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[1]);
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
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            workspaceId: ProjectData.Workspaces[0].Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Get($"api/projects?workspaceId={request.WorkspaceId}");
    }
}