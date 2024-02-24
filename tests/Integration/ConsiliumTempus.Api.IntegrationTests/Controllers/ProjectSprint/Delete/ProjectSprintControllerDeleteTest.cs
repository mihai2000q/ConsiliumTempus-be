using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Delete;

public class ProjectSprintControllerDeleteTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "ProjectSprint")
{
    [Fact]
    public async Task WhenProjectSprintDeleteSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.DeleteAsync($"api/projects/sprints/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectSprintResponse>();
        response!.Message.Should().Be("Project Sprint has been deleted successfully!");
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenProjectSprintDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/projects/sprints/{id}");

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(1);
        dbContext.ProjectSprints.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == new Guid(id)).Should().BeNull();
    }
}