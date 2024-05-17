using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.Get;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Get;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerGetTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task GetProjectTask_WhenSuccessful_ShouldReturnTask()
    {
        // Arrange
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateGetProjectTaskRequest(
            task.Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Get($"api/projects/tasks/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetProjectTaskResponse>();
        Utils.ProjectTask.AssertGetResponse(
            response!,
            task);
    }

    [Fact]
    public async Task GetProjectTask_WhenProjectIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetProjectTaskRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/projects/tasks/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.NotFound);
    }
}