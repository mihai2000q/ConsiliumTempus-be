using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities;
using ConsiliumTempus.Domain.Common.Errors;

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
            project.Sprints);
    }
    
    [Fact]
    public async Task GetCollectionProjectSprint_WhenProjectIsNotFound_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/projects/sprints?projectId={request.ProjectId}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);
    }
}