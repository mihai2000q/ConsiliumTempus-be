using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollectionForWorkspace;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionForWorkspaceTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task WhenGetCollectionForWorkspaceSucceeds_ShouldReturnProjects()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectForWorkspaceRequest(
            ProjectData.Workspaces.First().Id.Value);
        
        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/workspace?workspaceId={request.WorkspaceId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectForWorkspaceResponse>();
        Utils.Project.AssertGetCollectionForWorkspaceResponse(
            response!,
            ProjectData.Projects
                .Where(p => p.Workspace.Id.Value == request.WorkspaceId));
    }

    [Fact]
    public async Task WhenGetCollectionForWorkspaceFails_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectForWorkspaceRequest();
        
        // Act
        var outcome = await Client.Get($"api/projects/workspace?workspaceId={request.WorkspaceId}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
    }
}