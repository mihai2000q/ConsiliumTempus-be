using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Stage.GetCollection;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.GetCollection;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerGetCollectionTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task GetCollectionProjectStage_WhenSuccessful_ShouldReturnProjectStages()
    {
        // Arrange
        var sprint = ProjectStageData.ProjectSprints.First();
        var request = ProjectStageRequestFactory.CreateGetCollectionProjectStageRequest(
            sprint.Id.Value);

        // Act
        Client.UseCustomToken(ProjectStageData.Users.First());
        var outcome = await Client.Get($"api/projects/stages" +
                                       $"?projectSprintId={request.ProjectSprintId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionProjectStageResponse>();
        Utils.ProjectStage.AssertGetCollectionResponse(
            response!,
            sprint.Stages);
    }
}