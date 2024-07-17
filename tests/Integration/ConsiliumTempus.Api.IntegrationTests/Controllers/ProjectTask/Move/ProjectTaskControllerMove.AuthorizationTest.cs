using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Move;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerMoveAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task MoveProjectTask_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0]);
    }

    [Fact]
    public async Task MoveProjectTask_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[3]);
    }

    [Fact]
    public async Task MoveProjectTask_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[4]);
    }

    [Fact]
    public async Task MoveProjectTask_WhenWithoutMembership_ShouldReturnForbiddenResponse()
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

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user)
    {
        // Arrange
        var task = ProjectTaskData.ProjectTasks.First();
        var overStage = ProjectTaskData.ProjectStages[2];
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            sprintId: task.Stage.Sprint.Id.Value,
            id: task.Id.Value,
            overId: overStage.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/projects/tasks/Move", request);
    }
}