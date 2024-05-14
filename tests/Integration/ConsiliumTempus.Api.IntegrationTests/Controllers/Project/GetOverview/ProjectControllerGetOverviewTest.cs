using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetOverview;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetOverviewTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetOverviewProject_WhenSucceeds_ShouldReturnProjectOverview()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateGetOverviewProjectRequest(
            project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/overview/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetOverviewProjectResponse>();
        Utils.Project.AssertGetOverviewProjectResponse(response!, project);
    }

    [Fact]
    public async Task GetOverviewProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetOverviewProjectRequest();

        // Act
        var outcome = await Client.Get($"api/projects/overview/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);
    }
}