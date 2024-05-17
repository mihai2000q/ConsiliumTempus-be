using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.GetCollection;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerGetCollectionValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task GetCollectionProjectTask_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            ProjectTaskData.Projects.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCollectionProjectTask_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateGetCollectionProjectTaskRequest(
            projectStageId: Guid.Empty);

        // Act
        var outcome = await Client.Get($"api/projects/tasks" +
                                       $"?projectStageId={request.ProjectStageId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}