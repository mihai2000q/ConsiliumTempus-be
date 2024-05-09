using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.Create;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerCreateAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task CreateProjectStage_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectStageData.Users[0]);
    }

    [Fact]
    public async Task CreateProjectStage_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectStageData.Users[3]);
    }

    [Fact]
    public async Task CreateProjectStage_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectStageData.Users[4]);
    }

    [Fact]
    public async Task CreateProjectStage_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectStageData.Users[1]);
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
        var sprint = ProjectStageData.ProjectSprints.First();
        var request = ProjectStageRequestFactory.CreateCreateProjectStageRequest(sprint.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Post("api/projects/stages", request);
    }
}