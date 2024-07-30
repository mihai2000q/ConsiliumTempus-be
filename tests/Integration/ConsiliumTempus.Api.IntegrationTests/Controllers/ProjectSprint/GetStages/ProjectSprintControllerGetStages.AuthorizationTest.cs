using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.GetStages;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetStagesAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    // Permission Authorization
    [Fact]
    public async Task GetStagesFromProjectSprint_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0]);
    }

    [Fact]
    public async Task GetStagesFromProjectSprint_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[3]);
    }

    [Fact]
    public async Task GetStagesFromProjectSprint_WhenWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[4]);
    }

    [Fact]
    public async Task GetStagesFromProjectSprint_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task GetStagesFromProjectSprint_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[4], ProjectSprintData.ProjectSprints[^3]);
    }

    [Fact]
    public async Task GetStagesFromProjectSprint_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0], ProjectSprintData.ProjectSprints[^2]);
    }

    [Fact]
    public async Task GetStagesFromProjectSprint_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
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
        var request = ProjectSprintRequestFactory.CreateGetStagesFromProjectSprintRequest(sprint.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Get($"api/projects/sprints/{request.Id}/stages");
    }
}