using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Delete;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerDeleteValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task DeleteProjectTask_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var stage = ProjectTaskData.ProjectStages.First();
        var task = stage.Tasks[0];
        var request = ProjectTaskRequestFactory.CreateDeleteProjectTaskRequest(
            task.Id.Value,
            stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Delete($"api/projects/tasks/" +
                                          $"{request.Id}/from/{request.StageId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteProjectTask_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateDeleteProjectTaskRequest(id: Guid.Empty);

        // Act
        var outcome = await Client.Delete($"api/projects/tasks/" +
                                          $"{request.Id}/from/{request.StageId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}