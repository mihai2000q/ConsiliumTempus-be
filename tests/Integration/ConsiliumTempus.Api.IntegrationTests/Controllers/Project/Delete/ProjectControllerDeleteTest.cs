using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Delete;

public class ProjectControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Project")
{
    [Fact]
    public async Task WhenProjectDeleteSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/projects/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectResponse>();
        response!.Message.Should().Be("Project has been deleted successfully!");
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().BeEmpty();
    }

    [Fact]
    public async Task WhenProjectDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/projects/{id}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(1);
        dbContext.Projects.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == new Guid(id)).Should().BeNull();
    }
}