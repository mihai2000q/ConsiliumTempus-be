using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Create;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerCreateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task CreateProjectSprint_WhenSucceeds_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectResponse>();
        response!.Message.Should().Be("Project Sprint has been created successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(ProjectSprintData.ProjectSprints.Length + 1);
        var createdSprint = await dbContext.ProjectSprints
            .Include(ps => ps.Project.Workspace)
            .SingleAsync(ps => ps.Name.Value == request.Name);
        Utils.ProjectSprint.AssertCreation(createdSprint, request);
    }

    [Fact]
    public async Task CreateProjectSprint_WhenProjectIsNotFound_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(ProjectSprintData.ProjectSprints.Length);
        dbContext.ProjectSprints.SingleOrDefault(p => p.Name.Value == request.Name)
            .Should().BeNull();
    }
}