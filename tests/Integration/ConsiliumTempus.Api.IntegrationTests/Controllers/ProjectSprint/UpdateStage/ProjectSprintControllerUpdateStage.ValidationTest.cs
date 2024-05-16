using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.UpdateStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerUpdateStageValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var stage = sprint.Stages[0];
        var request = ProjectSprintRequestFactory.CreateUpdateStageFromProjectSprintRequest(
            id: sprint.Id.Value,
            stageId: stage.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Put("api/projects/sprints/Update-Stage", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateStageFromProjectSprint_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateStageFromProjectSprintRequest(
            id: Guid.Empty, 
            name: string.Empty);  

        // Act
        var outcome = await Client.Put("api/projects/sprints/Update-Stage", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}