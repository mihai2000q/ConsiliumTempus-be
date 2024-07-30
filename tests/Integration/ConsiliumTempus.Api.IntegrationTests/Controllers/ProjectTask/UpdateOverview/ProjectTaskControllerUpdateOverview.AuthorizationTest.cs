using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.UpdateOverview;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerUpdateOverviewAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    // Permission Authorization
    [Fact]
    public async Task UpdateOverviewProjectTask_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0]);
    }

    [Fact]
    public async Task UpdateOverviewProjectTask_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[3]);
    }

    [Fact]
    public async Task UpdateOverviewProjectTask_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[4]);
    }

    [Fact]
    public async Task UpdateOverviewProjectTask_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task UpdateOverviewProjectTask_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[4], ProjectTaskData.ProjectTasks[^3]);
    }

    [Fact]
    public async Task UpdateOverviewProjectTask_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0], ProjectTaskData.ProjectTasks[^2]);
    }

    [Fact]
    public async Task UpdateOverviewProjectTask_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[0], ProjectTaskData.ProjectTasks[^1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user, ProjectTaskAggregate? task = null)
    {
        var outcome = await ArrangeAndAct(user, task);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user, ProjectTaskAggregate? task = null)
    {
        var outcome = await ArrangeAndAct(user, task);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user, ProjectTaskAggregate? task = null)
    {
        // Arrange
        task ??= ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateUpdateOverviewProjectTaskRequest(task.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/projects/tasks/overview", request);
    }
}