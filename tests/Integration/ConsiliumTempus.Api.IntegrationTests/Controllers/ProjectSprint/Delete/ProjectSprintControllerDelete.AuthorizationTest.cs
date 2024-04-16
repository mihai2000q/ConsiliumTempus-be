using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Delete;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerDeleteAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task DeleteProjectSprint_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0]);
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[3]);
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[4]);
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[1]);
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
        var sprint = ProjectSprintData.ProjectSprints.First();

        // Act
        Client.UseCustomToken(user);
        return await Client.Delete($"api/projects/sprints/{sprint.Id.Value}");
    }
}