using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.GetCollection;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetCollectionValidationTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task WhenGetCollectionForWorkspaceQueryIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            ProjectSprintData.Projects.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/sprints?projectId={request.ProjectId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WhenGetCollectionForWorkspaceQueryIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(Guid.Empty);
        
        // Act
        var outcome = await Client.Get($"api/projects/sprints?projectId={request.ProjectId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}