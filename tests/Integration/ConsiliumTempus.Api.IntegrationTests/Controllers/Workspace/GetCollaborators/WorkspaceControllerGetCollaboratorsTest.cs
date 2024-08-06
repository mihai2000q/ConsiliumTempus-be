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
    public async Task GetCollaboratorsFromWorkspace_WhenIsSuccessful_ShouldReturnCollaborators()
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

        Utils.Workspace.AssertGetCollaboratorsResponse(
            response!,
            workspace.Memberships,
            workspace.Memberships.Count);
    }

    [Fact]
    public async Task GetCollaboratorsFromWorkspace_WhenRequestHasOrderBy_ShouldReturnOrderedCollaborators()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest(
            id: workspace.Id.Value,
            orderBy: ["workspace_role_id.desc", "user_name.asc"]);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces/{request.Id}/collaborators" +
                                       $"?{request.OrderBy!.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollaboratorsFromWorkspaceResponse>();

        var expectedCollaborators = workspace.Memberships
            .OrderByDescending(m => m.WorkspaceRole.Id)
            .ThenBy(m => m.User.Name.Value)
            .ToList();

        Utils.Workspace.AssertGetCollaboratorsResponse(
            response!,
            expectedCollaborators,
            expectedCollaborators.Count,
            true);
    }
    
    [Fact]
    public async Task GetCollaboratorsFromWorkspace_WhenRequestHasOrderByAndPagination_ShouldReturnOrderedAndPaginatedCollaborators()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest(
            id: workspace.Id.Value,
            pageSize: 2,
            currentPage: 1,
            orderBy: ["workspace_role_id.desc", "user_email.asc"]);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces/{request.Id}/collaborators" +
                                       $"?{request.OrderBy!.ToOrderByQueryParam()}" +
                                       $"&currentPage={request.CurrentPage}&pageSize={request.PageSize}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollaboratorsFromWorkspaceResponse>();

        var expectedCollaborators = workspace.Memberships
            .OrderByDescending(m => m.WorkspaceRole.Id)
            .ThenBy(m => m.User.Credentials.Email)
            .Skip((request.CurrentPage!.Value - 1) * request.PageSize!.Value)
            .Take(request.PageSize!.Value)
            .ToList();

        Utils.Workspace.AssertGetCollaboratorsResponse(
            response!,
            expectedCollaborators,
            workspace.Memberships.Count,
            true);
    }

    [Fact]
    public async Task GetCollaboratorsFromWorkspace_WhenRequestHasSearch_ShouldReturnFilteredCollaborators()
    {
        // Arrange
        var workspace = WorkspaceData.Workspaces.First();
        var request = WorkspaceRequestFactory.CreateGetCollaboratorsFromWorkspaceRequest(
            id: workspace.Id.Value,
            search: ["user_name sw Michael", "workspace_role_name eq Admin"]);

        // Act
        Client.UseCustomToken(WorkspaceData.Users.First());
        var outcome = await Client.Get($"api/workspaces/{request.Id}/collaborators" +
                                       $"?{request.Search!.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollaboratorsFromWorkspaceResponse>();

        var expectedCollaborators = workspace.Memberships
            .Where(m => m.User.Name.Value.StartsWith("Michael"))
            .Where(m => m.WorkspaceRole.Name == "Admin")
            .ToList();

        Utils.Workspace.AssertGetCollaboratorsResponse(
            response!,
            expectedCollaborators,
            workspace.Memberships.Count);
    }

    [Fact]
    public async Task GetCollaboratorsFromWorkspace_WhenRequestHasSearchValue_ShouldReturnFilteredCollaborators()
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
            .Where(m => m.User.Name.Value.Contains(request.SearchValue!))
            .ToList();

        Utils.Workspace.AssertGetCollaboratorsResponse(
            response!,
            expectedCollaborators,
            expectedCollaborators.Count);
    }

    [Fact]
    public async Task GetCollaboratorsFromWorkspace_WhenWorkspaceIsNotFound_ShouldReturnEmpty()
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