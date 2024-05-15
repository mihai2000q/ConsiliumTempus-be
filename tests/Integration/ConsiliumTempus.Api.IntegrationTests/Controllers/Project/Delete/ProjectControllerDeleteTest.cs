using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Delete;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerDeleteTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task DeleteProject_WhenSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateDeleteProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Delete($"api/projects/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectResponse>();
        response!.Message.Should().Be("Project has been deleted successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(ProjectData.Projects.Length - 1);
        var workspace = dbContext.Workspaces.Single(w => w == project.Workspace);
        workspace.LastActivity.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }

    [Fact]
    public async Task DeleteProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateDeleteProjectRequest();

        // Act
        var outcome = await Client.Delete($"api/projects/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(ProjectData.Projects.Length);
        dbContext.Projects.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == request.Id).Should().BeNull();
    }
}