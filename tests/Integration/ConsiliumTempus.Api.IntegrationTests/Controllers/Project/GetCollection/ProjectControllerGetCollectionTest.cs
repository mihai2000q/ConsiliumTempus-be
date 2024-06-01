using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollection;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetCollectionProject_WhenRequestIsEmpty_ShouldReturnUserProjects()
    {
        // Arrange
        var user = ProjectData.Users.First();

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get("api/projects");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = ProjectData.Projects
            .Where(p => p.Workspace.Memberships.Any(m => m.User == user))
            .ToList();
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasWorkspaceId_ShouldReturnProjectsFilteredByWorkspace()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            workspaceId: ProjectData.Workspaces.First().Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?workspaceId={request.WorkspaceId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = GetFilteredProjects(user, p =>
            p.Workspace.Id.Value == request.WorkspaceId);
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasSearchNameContains_ShouldReturnProjectsFilteredByName()
    {
        // Arrange
        const string searchName = "win";
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            search: [$"name ct {searchName}"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?{request.Search?.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = GetFilteredProjects(user, p =>
            p.Name.Value.ToLower().Contains(searchName));
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasSearchIsFavoriteEqual_ShouldReturnProjectsFilteredByIsFavorite()
    {
        // Arrange
        const bool searchIsFavorite = true;
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            search: [$"is_favorite eq {searchIsFavorite}"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?{request.Search?.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = GetFilteredProjects(user, p => 
            p.IsFavorite.Value == searchIsFavorite);
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasSearchIsPrivateEqual_ShouldReturnProjectsFilteredByIsPrivate()
    {
        // Arrange
        const bool searchIsPrivate = false;
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            search: [$"is_private eq {searchIsPrivate}"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?{request.Search?.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = GetFilteredProjects(user, p =>
            p.IsPrivate.Value == searchIsPrivate);
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasNameDescOrder_ShouldReturnProjectsOrderedByDescendingName()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            orderBy: ["name.desc"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?{request.OrderBy?.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = ProjectData.Projects
            .Where(p => p.Workspace.Memberships.Any(m => m.User == user))
            .OrderByDescending(p => p.Name.Value)
            .ToList();
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null,
            true);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasNameDescAndLastActivityAscOrder_ShouldReturnProjectsOrderedByDescendingName()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            orderBy: ["name.desc", " last_activity.asc"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?{request.OrderBy?.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = ProjectData.Projects
            .Where(p => p.Workspace.Memberships.Any(m => m.User == user))
            .OrderByDescending(p => p.Name.Value)
            .ThenBy(p => p.LastActivity)
            .ToList();
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null,
            true);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasPaginationAndOrder_ShouldReturnProjectsOrderedAndPaginated()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            pageSize: 2,
            currentPage: 2,
            orderBy: ["name.asc"]);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects" +
                                       $"?pageSize={request.PageSize}" +
                                       $"&currentPage={request.CurrentPage}" +
                                       $"&{request.OrderBy?.ToOrderByQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();

        var userProjects = ProjectData.Projects
            .Where(p => p.Workspace.Memberships.Any(m => m.User == user))
            .ToList();
        var expectedProjects = userProjects
            .OrderBy(p => p.Name.Value)
            .Skip(request.PageSize!.Value * (request.CurrentPage!.Value - 1))
            .Take(request.PageSize.Value)
            .ToList();

        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            userProjects.Count,
            (int)Math.Ceiling((double)userProjects.Count / request.PageSize.Value),
            true);
    }

    private static List<ProjectAggregate> GetFilteredProjects(
        UserAggregate user,
        Func<ProjectAggregate, bool> whereSelector)
    {
        return ProjectData.Projects
            .Where(p => p.Workspace.Memberships.Any(m => m.User == user))
            .Where(whereSelector)
            .ToList();
    }
}