using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.GetCollection;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetCollectionTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task GetCollectionProjectSprint_WhenSuccessful_ShouldReturnSprints()
    {
        // Arrange
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            project.Id.Value);

        // Act
        Client.UseCustomToken(project.Workspace.Memberships[0].User);
        var outcome = await Client.Get($"api/projects/sprints?projectId={request.ProjectId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectSprintResponse>();
        Utils.ProjectSprint.AssertGetCollectionResponse(
            response!,
            project.Sprints,
            project.Sprints.Count);
    }
    
    [Fact]
    public async Task GetCollectionProjectSprint_WhenRequestHasSearchNameContains_ShouldReturnSprints()
    {
        // Arrange
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            project.Id.Value,
            search: ["name sw sprint 1"]);

        // Act
        Client.UseCustomToken(project.Workspace.Memberships[0].User);
        var outcome = await Client.Get($"api/projects/sprints" +
                                       $"?projectId={request.ProjectId}" +
                                       $"&{request.Search!.ToSearchQueryParam()}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectSprintResponse>();

        var expectedSprints = project.Sprints
            .Where(ps => ps.Name.Value.ToLower().StartsWith("sprint 1"))
            .ToList();
        
        Utils.ProjectSprint.AssertGetCollectionResponse(
            response!,
            expectedSprints,
            expectedSprints.Count);
    }
    
    [Fact]
    public async Task GetCollectionProjectSprint_WhenRequestHasSearchNameContainsAndFromThisYear_ShouldReturnSprints()
    {
        // Arrange
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            project.Id.Value,
            search: ["name ct sprint 1"],
            fromThisYear: true);

        // Act
        Client.UseCustomToken(project.Workspace.Memberships[0].User);
        var outcome = await Client.Get($"api/projects/sprints" +
                                       $"?projectId={request.ProjectId}" +
                                       $"&{request.Search!.ToSearchQueryParam()}" +
                                       $"&fromThisYear={request.FromThisYear}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectSprintResponse>();
        
        var date = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var expectedSprints = project.Sprints
            .Where(ps => ps.Name.Value.ToLower().Contains("sprint 1"))
            .Where(ps => 
                ps.StartDate >= DateOnly.FromDateTime(date) ||
                ps.EndDate >= DateOnly.FromDateTime(date) ||
                ps.Audit.CreatedDateTime >= date)
            .ToList();
        
        Utils.ProjectSprint.AssertGetCollectionResponse(
            response!,
            expectedSprints,
            expectedSprints.Count);
    }
    
    [Fact]
    public async Task GetCollectionProjectSprint_WhenRequestHasFromThisYear_ShouldReturnSprints()
    {
        // Arrange
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            project.Id.Value,
            fromThisYear: true);

        // Act
        Client.UseCustomToken(project.Workspace.Memberships[0].User);
        var outcome = await Client.Get($"api/projects/sprints" +
                                       $"?projectId={request.ProjectId}" +
                                       $"&fromThisYear={request.FromThisYear}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectSprintResponse>();
        
        var date = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var expectedSprints = project.Sprints
            .Where(ps => 
                ps.StartDate >= DateOnly.FromDateTime(date) ||
                ps.EndDate >= DateOnly.FromDateTime(date) ||
                ps.Audit.CreatedDateTime >= date)
            .ToList();
        
        Utils.ProjectSprint.AssertGetCollectionResponse(
            response!,
            expectedSprints,
            expectedSprints.Count);
    }
    
    [Fact]
    public async Task GetCollectionProjectSprint_WhenRequestHasFromThisYearAndInitialEmptyReturn_ShouldReturnSprints()
    {
        // Arrange
        var project = ProjectSprintData.Projects[3];
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            project.Id.Value,
            fromThisYear: true);

        // Act
        Client.UseCustomToken(project.Workspace.Memberships[0].User);
        var outcome = await Client.Get($"api/projects/sprints" +
                                       $"?projectId={request.ProjectId}" +
                                       $"&fromThisYear={request.FromThisYear}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectSprintResponse>();
        
        Utils.ProjectSprint.AssertGetCollectionResponse(
            response!,
            [project.Sprints[^1]],
            1);
    }

    [Fact]
    public async Task GetCollectionProjectSprint_WhenProjectIsNotFound_ShouldReturnEmptySprints()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/projects/sprints?projectId={request.ProjectId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectSprintResponse>();
        response!.Sprints.Should().BeEmpty();
    }
}