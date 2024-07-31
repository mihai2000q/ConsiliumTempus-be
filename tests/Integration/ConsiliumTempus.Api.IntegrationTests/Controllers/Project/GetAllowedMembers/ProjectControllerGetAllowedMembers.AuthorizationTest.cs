using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetAllowedMembers;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetAllowedMembersAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    // Permission Authorization
    [Fact]
    public async Task GetAllowedMembersFromProject_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[0]);
    }

    [Fact]
    public async Task GetAllowedMembersFromProject_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[3]);
    }

    [Fact]
    public async Task GetAllowedMembersFromProject_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[4]);
    }

    [Fact]
    public async Task GetAllowedMembersFromProject_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task GetAllowedMembersFromProject_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[4], ProjectData.Projects[^3]);
    }

    [Fact]
    public async Task GetAllowedMembersFromProject_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[0], ProjectData.Projects[^2]);
    }

    [Fact]
    public async Task GetAllowedMembersFromProject_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[0], ProjectData.Projects[^1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user, ProjectAggregate? project = null)
    {
        var outcome = await ArrangeAndAct(user, project);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user, ProjectAggregate? project = null)
    {
        var outcome = await ArrangeAndAct(user, project);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user, ProjectAggregate? project = null)
    {
        // Arrange
        project ??= ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateGetAllowedMembersFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Get($"api/projects/{request.Id}/allowed-members");
    }
}