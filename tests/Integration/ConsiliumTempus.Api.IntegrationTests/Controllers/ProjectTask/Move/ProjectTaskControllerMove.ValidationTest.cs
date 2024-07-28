using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Move;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerMoveValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task MoveProjectTask_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var task = ProjectTaskData.ProjectTasks.First();
        var overStage = ProjectTaskData.ProjectStages[2];
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            id: task.Id.Value,
            overId: overStage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Post("api/projects/tasks/move", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MoveProjectTask_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateMoveProjectTaskRequest(
            id: Guid.Empty, 
            overId: Guid.Empty);  

        // Act
        var outcome = await Client.Post("api/projects/tasks/move", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}