using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Delete;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerDeleteTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task WhenProjectDeleteSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        
        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Delete($"api/projects/{project.Id}");

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
    public async Task WhenProjectDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var outcome = await Client.Delete($"api/projects/{id}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);
        
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(ProjectData.Projects.Length);
        dbContext.Projects.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == id).Should().BeNull();
    }
}