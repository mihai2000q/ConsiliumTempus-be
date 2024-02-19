using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint;

public class ProjectSprintControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "ProjectSprint")
{
    [Fact]
    public async Task WhenProjectSprintDeleteWithAdminRole_ShouldDeleteAndReturnSuccessResponse()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenProjectSprintDeleteWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenProjectSprintDeleteWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenProjectSprintDeleteWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    [Fact]
    public async Task WhenProjectSprintDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/projects/sprints/{id}");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(1);
        dbContext.ProjectSprints.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == new Guid(id)).Should().BeNull();
        
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }

    private async Task AssertSuccessfulRequest(string email)
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/projects/sprints/{id}");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().BeEmpty();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectSprintResponse>();
        response!.Message.Should().Be("Project Sprint has been deleted successfully!");
    }
    
    private async Task AssertForbiddenResponse(string email)
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/projects/sprints/{id}");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(1);
        dbContext.ProjectSprints.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == new Guid(id)).Should().NotBeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}