using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Sprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Create;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerCreateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task CreateProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(
            projectId: Guid.Empty, 
            name: string.Empty);  

        // Act
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}