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
    public async Task GetProjectCollection_WhenRequestIsEmpty_ShouldReturnUserProjects()
    {
        // Arrange
        var user = ProjectData.Users.First();

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get("api/projects");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectResponse>();
        Utils.Project.AssertGetCollectionResponse(
            response!, 
            GetProjects(user));
    }
    
    [Fact]
    public async Task GetProjectCollection_WhenRequestHasWorkspaceId_ShouldReturnProjectsFilteredByWorkspace()
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
        Utils.Project.AssertGetCollectionResponse(
            response!,
            GetProjects(user, p => p.Workspace.Id.Value == request.WorkspaceId));
    }
    
    [Fact]
    public async Task GetProjectCollection_WhenRequestHasName_ShouldReturnProjectsFilteredByName()
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
        Utils.Project.AssertGetCollectionResponse(
            response!,
            GetProjects(user, p => p.Workspace.Id.Value == request.WorkspaceId));
    }
    
    [Fact]
    public async Task GetProjectCollection_WhenRequestHasIsFavorite_ShouldReturnProjectsFilteredByIsFavorite()
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
        Utils.Project.AssertGetCollectionResponse(
            response!,
            GetProjects(user, p => p.IsFavorite.Value == request.IsFavorite));
    }
    
    [Fact]
    public async Task GetProjectCollection_WhenRequestHasIsPrivate_ShouldReturnProjectsFilteredByIsPrivate()
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
        Utils.Project.AssertGetCollectionResponse(
            response!,
            GetProjects(user, p => p.IsPrivate.Value == request.IsPrivate));
    }

    private static IEnumerable<ProjectAggregate> GetProjects(
        UserAggregate user,
        Func<ProjectAggregate, bool>? whereSelector = null)
    {
        var projects = ProjectData.Projects
            .Where(p => p.Workspace.Memberships.Any(m => m.User == user));
        return whereSelector is not null 
            ? projects.Where(whereSelector) 
            : projects;
    }
}