using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollectionForWorkspace;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionForWorkspaceValidationTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task WhenGetCollectionForWorkspaceQueryIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectForWorkspaceRequest(
            ProjectData.Workspaces.First().Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/workspace?workspaceId={request.WorkspaceId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WhenGetCollectionForWorkspaceQueryIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectForWorkspaceRequest(Guid.Empty);
        
        // Act
        var outcome = await Client.Get($"api/projects/workspace?workspaceId={request.WorkspaceId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}