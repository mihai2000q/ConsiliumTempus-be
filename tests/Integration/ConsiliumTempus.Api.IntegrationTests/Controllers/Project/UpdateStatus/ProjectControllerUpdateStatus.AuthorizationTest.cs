using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateStatus;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerDeleteAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateStatus_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(ProjectData.Users[0]);
    }

    [Fact]
    public async Task UpdateStatus_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(ProjectData.Users[3]);
    }

    [Fact]
    public async Task UpdateStatus_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[4]);
    }

    [Fact]
    public async Task UpdateStatus_WhenWithoutMembership_ShouldReturnForbiddenResponse()
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
        var project = ProjectData.Projects.First();
        var status = project.Statuses[0];
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest(
            project.Id.Value,
            status.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/projects/Update-Status/", request);
    }
}