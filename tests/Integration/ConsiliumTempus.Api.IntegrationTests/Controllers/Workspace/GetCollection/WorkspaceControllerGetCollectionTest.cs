using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.GetCollection;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Workspace;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetCollection;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetCollectionTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task GetCollectionWorkspace_WhenUserIsNotFound_ShouldReturnUserNotFoundError()
    {
        // Arrange

        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Get("api/Workspaces");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenIsSuccessful_ShouldReturnWorkspaces()
    {
        // Arrange
        var user = WorkspaceData.Users.First();

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get("api/Workspaces");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionWorkspaceResponse>();
        var expectedWorkspaces = user.Memberships.Select(m => m.Workspace).ToList();
        Utils.Workspace.AssertGetCollectionResponse(
            response!,
            expectedWorkspaces,
            expectedWorkspaces.Count,
            user);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenRequestHasIsPersonalWorkspaceFirst_ShouldReturnWorkspacesWithPersonalWorkspaceFirst()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest(
            isPersonalWorkspaceFirst: true);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/Workspaces" +
                                       $"?isPersonalWorkspaceFirst={request.IsPersonalWorkspaceFirst}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionWorkspaceResponse>();
        var expectedWorkspaces = user.Memberships.Select(m => m.Workspace).ToList();

        response!.Workspaces.First().IsPersonal.Should().BeTrue();

        response.Workspaces.First().Owner.Id.Should().Be(user.Id.Value);
        response.Workspaces.First().Owner.Name.Should().Be(user.FirstName.Value + " " + user.LastName.Value);

        Utils.Workspace.AssertGetCollectionResponse(
            response,
            expectedWorkspaces,
            expectedWorkspaces.Count,
            user);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenRequestHasSearchNameContains_ShouldReturnWorkspacesFilteredByName()
    {
        // Arrange
        const string searchName = "sOme";
        var user = WorkspaceData.Users.First();
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest(
            search: [$"name ct {searchName}"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/Workspaces?{request.Search?.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionWorkspaceResponse>();
        var expectedWorkspaces = user.Memberships.Select(m => m.Workspace)
            .Where(w => w.Name.Value.ToLower().Contains(searchName.ToLower()))
            .ToList();
        Utils.Workspace.AssertGetCollectionResponse(
            response!,
            expectedWorkspaces,
            expectedWorkspaces.Count,
            user);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenRequestHasOrderNameAsc_ShouldReturnWorkspacesOrderedByNameAscending()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest(
            orderBy: ["name.asc"]);
        
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/Workspaces?{request.OrderBy?.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionWorkspaceResponse>();
        var expectedWorkspaces = user.Memberships.Select(m => m.Workspace)
            .OrderBy(w => w.Name.Value)
            .ToList();
        Utils.Workspace.AssertGetCollectionResponse(
            response!,
            expectedWorkspaces,
            expectedWorkspaces.Count,
            user,
            isOrdered: true);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenRequestHasNameAscAndLastActivityAscOrder_ShouldReturnWorkspacesOrderedByNameAscending()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest(
            orderBy: ["name.asc", "last_activity.asc"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/Workspaces?{request.OrderBy?.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionWorkspaceResponse>();
        var expectedWorkspaces = user.Memberships.Select(m => m.Workspace)
            .OrderBy(w => w.Name.Value)
            .ThenBy(w => w.LastActivity)
            .ToList();
        Utils.Workspace.AssertGetCollectionResponse(
            response!,
            expectedWorkspaces,
            expectedWorkspaces.Count,
            user,
            isOrdered: true);
    }

    [Fact]
    public async Task GetCollectionWorkspace_WhenRequestHasPaginationAndOrder_ShouldReturnWorkspacesPaginatedAndOrdered()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var request = WorkspaceRequestFactory.CreateGetCollectionWorkspaceRequest(
            orderBy: ["name.desc"],
            pageSize: 2,
            currentPage: 1);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/Workspaces" +
                                       $"?{request.OrderBy?.ToOrderByQueryParam()}" +
                                       $"&pageSize={request.PageSize}" +
                                       $"&currentPage={request.CurrentPage}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionWorkspaceResponse>();

        var workspaces = user.Memberships.Select(m => m.Workspace)
            .OrderByDescending(w => w.Name.Value)
            .ToList();
        var expectedWorkspaces = workspaces
            .Skip(request.PageSize!.Value * (request.CurrentPage!.Value - 1))
            .Take(request.PageSize.Value)
            .ToList();

        Utils.Workspace.AssertGetCollectionResponse(
            response!,
            expectedWorkspaces,
            workspaces.Count,
            user,
            isOrdered: true);
    }
}