using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities.Stage;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectStage.Create;

[Collection(nameof(ProjectStageControllerCollection))]
public class ProjectStageControllerCreateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectStageData())
{
    [Fact]
    public async Task CreateProjectStage_WhenSucceeds_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var projectSprint = ProjectStageData.ProjectSprints.First();
        var request = ProjectStageRequestFactory.CreateCreateProjectStageRequest(projectSprint.Id.Value);

        // Act
        Client.UseCustomToken(ProjectStageData.Users.First());
        var outcome = await Client.Post("api/projects/stages", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectResponse>();
        response!.Message.Should().Be("Project Stage has been created successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectStages.Should().HaveCount(ProjectStageData.ProjectStages.Length + 1);
        var createdStage = await dbContext.ProjectStages
            .Include(ps => ps.Sprint)
            .ThenInclude(ps => ps.Project)
            .ThenInclude(p => p.Workspace)
            .SingleAsync(ps => ps.Name.Value == request.Name);
        Utils.ProjectStage.AssertCreation(createdStage, request);
    }

    [Fact]
    public async Task CreateProjectStage_WhenProjectSprintIsNotFound_ShouldReturnProjectSprintNotFoundError()
    {
        // Arrange
        var request = ProjectStageRequestFactory.CreateCreateProjectStageRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/projects/stages", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectStages.Should().HaveCount(ProjectStageData.ProjectStages.Length);
        dbContext.ProjectStages.SingleOrDefault(p => p.Name.Value == request.Name)
            .Should().BeNull();
    }
}