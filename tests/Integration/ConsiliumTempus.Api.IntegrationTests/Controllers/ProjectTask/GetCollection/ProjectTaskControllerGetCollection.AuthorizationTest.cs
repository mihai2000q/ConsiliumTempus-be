using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.GetCollection;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerGetCollectionAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task GetCollectionProjectTask_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0]);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[3]);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[4]);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[1]);
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
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            ProjectTaskData.ProjectStages.First().Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Get($"api/projects/tasks?projectStageId={request.ProjectStageId}");
    }
}