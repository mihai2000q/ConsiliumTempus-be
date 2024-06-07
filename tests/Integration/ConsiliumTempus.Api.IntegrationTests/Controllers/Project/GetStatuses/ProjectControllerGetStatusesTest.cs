using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.GetStatuses;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetStatuses;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetStatusesTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetStatusesFromProject_WhenSucceeds_ShouldReturnStatusesFromProject()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateGetStatusesFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/{request.Id}/Statuses");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetStatusesFromProjectResponse>();
        Utils.Project.AssertGetStatusesResponse(response!, project.Statuses, project.Statuses.Count);
    }

    [Fact]
    public async Task GetStatusesFromProject_WhenProjectIsNotFound_ShouldReturnEmptyStatuses()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetStatusesFromProjectRequest();

        // Act
        var outcome = await Client.Get($"api/projects/{request.Id}/Statuses");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetStatusesFromProjectResponse>();
        response!.Statuses.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
    }
}