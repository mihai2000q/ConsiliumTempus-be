using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.GetStages;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetStagesAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
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

    private Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user)
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetStagesFromProjectSprintRequest(
            ProjectSprintData.ProjectSprints.First().Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Get($"api/projects/sprints/{request.Id}/stages");
    }
}