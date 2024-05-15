using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.GetCollection;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerGetCollectionAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task GetCollectionProjectStage_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectStageData.Users[0]);
    }

    [Fact]
    public async Task GetCollectionProjectStage_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectStageData.Users[3]);
    }

    [Fact]
    public async Task GetCollectionProjectStage_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertSuccessfulResponse(ProjectStageData.Users[4]);
    }

    [Fact]
    public async Task GetCollectionProjectStage_WhenWithoutMembership_ShouldReturnForbiddenResponse()
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

    private Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user)
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateGetCollectionProjectStageRequest(
            ProjectStageData.ProjectSprints.First().Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Get($"api/projects/stages?projectSprintId={request.ProjectSprintId}");
    }
}