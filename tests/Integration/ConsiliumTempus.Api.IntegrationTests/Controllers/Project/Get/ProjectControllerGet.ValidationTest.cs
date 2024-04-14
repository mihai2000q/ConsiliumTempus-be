using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Get;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetValidationTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task WhenGetProjectQueryIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetProjectRequest(
            ProjectData.Projects.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WhenGetProjectQueryIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetProjectRequest(Guid.Empty);
        
        // Act
        var outcome = await Client.Get($"api/projects/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}