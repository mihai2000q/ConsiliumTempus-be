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
    public async Task GetCollectionProjectTask_WhenRequestDoesNotHaveOrderCustomOrderIsInPlace_ShouldReturnTasks()
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

        var count = 0;
        stage.Tasks.Should().AllSatisfy(t => t.CustomOrderPosition.Value.Should().Be(count++));
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenRequestHasNameAscendingOrder_ShouldReturnOrderedTasks()
    {
        // Arrange
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            stage.Id.Value,
            orderBy: ["name.asc"]);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}" +
                                       $"&{request.OrderBy?.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectTaskResponse>();

        var expectedTasks = stage.Tasks
            .OrderBy(t => t.Name.Value)
            .ToList();

        Utils.ProjectTask.AssertGetCollectionResponse(
            response!,
            expectedTasks,
            expectedTasks.Count);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenRequestHasNameContainsFilter_ShouldReturnFilteredTasks()
    {
        // Arrange
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            stage.Id.Value,
            search: ["name ct should"]);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}" +
                                       $"&{request.Search?.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectTaskResponse>();

        var expectedTasks = stage.Tasks
            .Where(t => t.Name.Value.ToLower().Contains("should"))
            .ToList();

        Utils.ProjectTask.AssertGetCollectionResponse(
            response!,
            expectedTasks,
            expectedTasks.Count);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenRequestHasIsCompletedEqualFilter_ShouldReturnFilteredTasks()
    {
        // Arrange
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            stage.Id.Value,
            search: ["is_completed eq true"]);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}" +
                                       $"&{request.Search?.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectTaskResponse>();

        var expectedTasks = stage.Tasks
            .Where(t => t.IsCompleted.Value)
            .ToList();

        Utils.ProjectTask.AssertGetCollectionResponse(
            response!,
            expectedTasks,
            expectedTasks.Count);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenRequestHasPaginationInfo_ShouldReturnPaginatedTasks()
    {
        // Arrange
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            stage.Id.Value,
            currentPage: 1,
            pageSize: 2);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}" +
                                       $"&currentPage={request.CurrentPage}" +
                                       $"&pageSize={request.PageSize}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectTaskResponse>();

        var expectedTasks = stage.Tasks
            .Skip(request.PageSize!.Value * (request.CurrentPage!.Value - 1))
            .Take(request.PageSize.Value)
            .ToList();

        Utils.ProjectTask.AssertGetCollectionResponse(
            response!,
            expectedTasks,
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