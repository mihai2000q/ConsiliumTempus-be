using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Create;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerCreateAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    // Permission Authorization
    [Fact]
    public async Task CreateProjectTask_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0]);
    }

    [Fact]
    public async Task CreateProjectTask_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[3]);
    }

    [Fact]
    public async Task CreateProjectTask_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[4]);
    }

    [Fact]
    public async Task CreateProjectTask_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task CreateProjectTask_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[4], ProjectTaskData.ProjectStages[3]);
    }

    [Fact]
    public async Task CreateProjectTask_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectTaskData.Users[0], ProjectTaskData.ProjectStages[4]);
    }

    [Fact]
    public async Task CreateProjectTask_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectTaskData.Users[0], ProjectTaskData.ProjectStages[5]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user, ProjectStage? stage = null)
    {
        var outcome = await ArrangeAndAct(user, stage);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user, ProjectStage? stage = null)
    {
        var outcome = await ArrangeAndAct(user, stage);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user, ProjectStage? stage = null)
    {
        // Arrange
        stage ??= ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest(stage.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Post("api/projects/tasks", request);
    }
}