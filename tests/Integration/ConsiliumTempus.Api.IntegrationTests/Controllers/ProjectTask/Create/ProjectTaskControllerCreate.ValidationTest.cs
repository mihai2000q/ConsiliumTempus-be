using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.Create;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerCreateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task CreateProjectTask_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var stage = ProjectTaskData.ProjectStages.First();
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest(stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Post("api/projects/tasks", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateProjectTask_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateCreateProjectTaskRequest(
            projectStageId: Guid.Empty, 
            name: string.Empty);  

        // Act
        var outcome = await Client.Post("api/projects/tasks", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}