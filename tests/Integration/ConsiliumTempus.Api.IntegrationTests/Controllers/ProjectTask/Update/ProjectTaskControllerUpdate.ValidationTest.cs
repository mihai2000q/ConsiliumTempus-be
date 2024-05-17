using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Update;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerUpdateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task UpdateProjectTask_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateUpdateProjectTaskRequest(task.Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Put("api/projects/tasks", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateProjectTask_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateProjectTaskRequest(
            id: Guid.Empty, 
            name: string.Empty);  

        // Act
        var outcome = await Client.Put("api/projects/tasks", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}