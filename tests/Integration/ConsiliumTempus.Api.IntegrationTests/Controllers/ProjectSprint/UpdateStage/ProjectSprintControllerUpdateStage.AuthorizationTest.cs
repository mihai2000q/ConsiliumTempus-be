using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.UpdateStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerUpdateStageAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    // Permission Authorization
    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0]);
    }

    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[3]);
    }

    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[4]);
    }

    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[4], ProjectSprintData.ProjectSprints[5]);
    }

    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0], ProjectSprintData.ProjectSprints[6]);
    }

    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[0], ProjectSprintData.ProjectSprints[7]);
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
        var request = ProjectSprintRequestFactory.CreateUpdateStageFromProjectSprintRequest(
            sprint.Id.Value,
            stage.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Put("api/projects/sprints/Update-Stage/", request);
    }
}