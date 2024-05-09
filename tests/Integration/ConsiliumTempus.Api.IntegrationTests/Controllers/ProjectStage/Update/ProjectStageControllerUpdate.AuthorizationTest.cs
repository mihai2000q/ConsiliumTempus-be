using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.Update;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerUpdateAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task UpdateProjectStage_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectStageData.Users[0]);
    }

    [Fact]
    public async Task UpdateProjectStage_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectStageData.Users[3]);
    }

    [Fact]
    public async Task UpdateProjectStage_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectStageData.Users[4]);
    }

    [Fact]
    public async Task UpdateProjectStage_WhenWithoutMembership_ShouldReturnForbiddenResponse()
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
        var request = ProjectStageRequestFactory.CreateUpdateProjectStageRequest(
            ProjectStageData.ProjectStages.First().Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Put("api/projects/Stages", request);
    }
}