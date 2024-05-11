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
    public async Task GetCollectionProject_WhenRequestHasName_ShouldReturnProjectsFilteredByName()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            name: "win");

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?name={request.Name}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = GetFilteredProjects(user, p =>
            p.Name.Value.ToLower().Contains(request.Name?.ToLower() ?? ""));
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasIsFavorite_ShouldReturnProjectsFilteredByIsFavorite()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            isFavorite: true);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?isFavorite={request.IsFavorite}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = GetFilteredProjects(user, p =>
            p.IsFavorite.Value == request.IsFavorite);
        Utils.Project.AssertGetCollectionResponse(
            response!,
            expectedProjects,
            expectedProjects.Count,
            null);
    }

    [Fact]
    public async Task GetCollectionProject_WhenRequestHasIsPrivate_ShouldReturnProjectsFilteredByIsPrivate()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            isPrivate: false);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?isPrivate={request.IsPrivate}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        var expectedProjects = GetFilteredProjects(user, p =>
            p.IsPrivate.Value == request.IsPrivate);
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
            order: "name.desc");

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?order={request.Order}");

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
    public async Task GetCollectionProject_WhenRequestHasPaginationAndOrder_ShouldReturnProjectsOrderedAndPaginated()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            pageSize: 2,
            currentPage: 2,
            order: "name.asc");

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects?pageSize={request.PageSize}" +
                                       $"&currentPage={request.CurrentPage}" +
                                       $"&order={request.Order}");

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