using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Get;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Sprint;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Get;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task GetProjectSprint_WhenSuccessful_ShouldReturnSprint()
    {
        // Arrange
        var projectSprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateGetProjectSprintRequest(
            projectSprint.Id.Value);

        // Act
        Client.UseCustomToken(projectSprint.Project.Workspace.Memberships[0].User);
        var outcome = await Client.Get($"api/projects/sprints/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetProjectSprintResponse>();
        Utils.ProjectSprint.AssertGetResponse(
            response!,
            projectSprint);
    }

    [Fact]
    public async Task GetProjectSprint_WhenProjectIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetProjectSprintRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/projects/sprints/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}