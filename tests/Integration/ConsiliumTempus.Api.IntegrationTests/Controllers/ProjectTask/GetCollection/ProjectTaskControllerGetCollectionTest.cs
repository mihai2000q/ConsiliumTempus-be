using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.GetCollection;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerGetCollectionTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task GetCollectionProjectTask_WhenSuccessful_ShouldReturnTasks()
    {
        // Arrange
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectTaskResponse>();
        Utils.ProjectTask.AssertGetCollectionResponse(
            response!,
            stage.Tasks,
            stage.Tasks.Count);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenStageIsNotFound_ShouldReturnEmptyTasks()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectTaskResponse>();
        response!.Tasks.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
    }
}