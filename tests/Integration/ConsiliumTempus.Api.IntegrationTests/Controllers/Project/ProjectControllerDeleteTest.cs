using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project;

public class ProjectControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Project")
{
    [Fact]
    public async Task WhenProjectDeleteWithAdminRole_ShouldReturnDeleteAndSuccessResponse()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenProjectDeleteWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenProjectDeleteWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenProjectDeleteWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    [Fact]
    public async Task WhenProjectDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "20000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/projects/{id}");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(1);
        dbContext.Projects.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == new Guid(id)).Should().BeNull();
        
        await outcome.ValidateError(Errors.Project.NotFound);
    }

    private async Task AssertSuccessfulRequest(string email)
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/projects/{id}");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().BeEmpty();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteProjectResponse>();
        response!.Message.Should().Be("Project has been deleted successfully!");
    }
    
    private async Task AssertForbiddenResponse(string email)
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/projects/{id}");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(1);
        dbContext.Projects.AsEnumerable()
            .SingleOrDefault(p => p.Id.Value == new Guid(id)).Should().NotBeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}