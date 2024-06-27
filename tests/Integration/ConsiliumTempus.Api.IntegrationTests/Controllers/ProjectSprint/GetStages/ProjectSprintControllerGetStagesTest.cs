using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.GetStages;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.GetStages;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetStagesStagesTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task GetStagesFromProjectSprint_WhenSuccessful_ShouldReturnSprint()
    {
        // Arrange
        var user = ProjectSprintData.Users.First();
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateGetStagesFromProjectSprintRequest(
            sprint.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects/sprints/{request.Id}/stages");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetStagesFromProjectSprintResponse>();
        Utils.ProjectSprint.AssertGetStagesResponse(
            response!,
            sprint.Stages);
    }

    [Fact]
    public async Task GetStagesProjectSprint_WhenIsNotFound_ShouldReturnEmptyStages()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetStagesFromProjectSprintRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/projects/sprints/{request.Id}/stages");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetStagesFromProjectSprintResponse>();
        response!.Stages.Should().BeEmpty();
    }
}