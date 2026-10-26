using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using ErrorOr;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Get;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerGetAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    // Permission Authorization
    [Fact]
    public async Task GetProjectTask_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0]);
    }

    [Fact]
    public async Task GetProjectTask_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[3]);
    }

    [Fact]
    public async Task GetProjectTask_WhenWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[4]);
    }

    [Fact]
    public async Task GetProjectTask_WhenWithoutMembership_ShouldReturnNotFoundError()
    {
        await AssertNotFoundResponse(Errors.ProjectTask.NotFound, ProjectTaskData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task GetProjectTask_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[4], ProjectTaskData.ProjectTasks[^3]);
    }

    [Fact]
    public async Task GetProjectTask_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0], ProjectTaskData.ProjectTasks[^2]);
    }

    [Fact]
    public async Task GetProjectTask_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnNotFoundError()
    {
        await AssertNotFoundResponse(Errors.ProjectTask.NotFound, ProjectTaskData.Users[0], ProjectTaskData.ProjectTasks[^1]);
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

    private async Task AssertNotFoundResponse(Error error, UserAggregate user, ProjectTaskAggregate? task = null)
    {
        var outcome = await ArrangeAndAct(user, task);

        // Assert
        await outcome.ValidateError(error);
    }

    private Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user, ProjectTaskAggregate? task = null)
    {
        // Arrange
        task ??= ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateGetProjectTaskRequest(task.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Get($"api/projects/tasks/{request.Id}");
    }
}