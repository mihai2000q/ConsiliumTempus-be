using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Delete;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerDeleteTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task DeleteProjectSprint_WhenSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();

        // Act
        Client.UseCustomToken(ProjectSprintData.Users.First());
        var outcome = await Client.Delete($"api/projects/sprints/{sprint.Id.Value}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectSprintResponse>();
        response!.Message.Should().Be("Project Sprint has been deleted successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(ProjectSprintData.ProjectSprints.Length - 1);
        (await dbContext.ProjectSprints.FindAsync(sprint.Id))
            .Should().BeNull();
        var project = dbContext.Projects
            .Include(p => p.Workspace)
            .Single(p => p == sprint.Project);
        project.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        project.Workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }

    [Fact]
    public async Task DeleteProjectSprint_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var outcome = await Client.Delete($"api/projects/sprints/{id}");

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(ProjectSprintData.ProjectSprints.Length);
        dbContext.ProjectSprints.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == id)
            .Should().BeNull();
    }
}