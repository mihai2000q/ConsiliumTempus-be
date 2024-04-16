using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Get;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetProject_WhenSucceeds_ShouldReturnProject()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateGetProjectRequest(
            project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetProjectResponse>();
        Utils.Project.AssertGetProjectResponse(response!, project);
    }

    [Fact]
    public async Task GetProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetProjectRequest();

        // Act
        var outcome = await Client.Get($"api/projects/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);
    }
}