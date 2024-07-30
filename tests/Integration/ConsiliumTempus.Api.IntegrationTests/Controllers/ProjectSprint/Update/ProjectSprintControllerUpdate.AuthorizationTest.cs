using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Update;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerUpdateAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    // Permission Authorization
    [Fact]
    public async Task UpdateProjectSprint_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0]);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[3]);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[4]);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task UpdateProjectSprint_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[4], ProjectSprintData.ProjectSprints[^3]);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0], ProjectSprintData.ProjectSprints[^2]);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
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
        var request = ProjectSprintRequestFactory.CreateUpdateProjectSprintRequest(sprint.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Put("api/projects/sprints", request);
    }
}