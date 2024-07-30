using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.RemoveStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerRemoveStageAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    // Permission Authorization
    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0]);
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[3]);
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[4]);
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[4], ProjectSprintData.ProjectSprints[^3]);
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0], ProjectSprintData.ProjectSprints[^2]);
    }

    [Fact]
    public async Task RemoveStageFromProjectSprint_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[0], ProjectSprintData.ProjectSprints[^1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user, ProjectSprintAggregate? sprint = null)
    {
        var outcome = await ArrangeAndAct(user, sprint);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user, ProjectSprintAggregate? sprint = null)
    {
        var outcome = await ArrangeAndAct(user, sprint);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user, ProjectSprintAggregate? sprint = null)
    {
        // Arrange
        sprint ??= ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[0];
        var request = ProjectSprintRequestFactory.CreateRemoveStageFromProjectSprintRequest(
            sprint.Id.Value,
            stage.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Delete($"api/projects/sprints/{request.Id}/Remove-Stage/{request.StageId}");
    }
}