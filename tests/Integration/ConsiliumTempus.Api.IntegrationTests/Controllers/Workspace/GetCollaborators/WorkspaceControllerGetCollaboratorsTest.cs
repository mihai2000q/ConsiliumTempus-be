using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollaborators;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetCollaborators;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetCollaboratorsTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetCollaborators_WhenIsSuccessful_ShouldReturnCollaborators()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest(
            id: workspace.Id.Value);
        
        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces/{request.Id}/collaborators");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollaboratorsFromWorkspaceResponse>();

        var expectedCollaborators = workspace.Memberships.Select(m => m.User);
        
        Utils.Workspace.AssertGetCollaboratorsResponse(
            response!,
            expectedCollaborators);
    }
    
    [Fact]
    public async Task GetCollaborators_WhenRequestHasSearchValue_ShouldReturnFilteredCollaborators()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest(
            id: workspace.Id.Value,
            searchValue: "Michael");
        
        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces/{request.Id}/collaborators" +
                                       $"?searchValue={request.SearchValue}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollaboratorsFromWorkspaceResponse>();

        var expectedCollaborators = workspace.Memberships
            .Select(m => m.User)
            .Where(u =>
                u.FirstName.Value.Contains(request.SearchValue!) || 
                u.LastName.Value.Contains(request.SearchValue!));
        
        Utils.Workspace.AssertGetCollaboratorsResponse(
            response!,
            expectedCollaborators);
    }

    [Fact]
    public async Task GetCollaborators_WhenWorkspaceIsNotFound_ShouldReturnEmpty()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest(
            id: Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/workspaces/{request.Id}/collaborators");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollaboratorsFromWorkspaceResponse>();
        response!.Collaborators.Should().BeEmpty();
    }
}