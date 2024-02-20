using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint;

public class ProjectSprintControllerCreateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "ProjectSprint")
{
    [Fact]
    public async Task WhenProjectSprintCreateWithAdminRole_ShouldCreateAndReturnSuccessResponse()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenProjectSprintCreateWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenProjectSprintCreateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenProjectSprintCreateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    [Fact]
    public async Task WhenProjectSprintCreateFails_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var request = new CreateProjectSprintRequest(
            new Guid("20000000-0000-0000-0000-000000000000"),
            "Sprint 2 - Qualify on Semi-Finals",
            null,
            null);
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/projects/sprints", request);

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(1);
        dbContext.ProjectSprints.SingleOrDefault(p => p.Name.Value == request.Name).Should().BeNull();
        
        await outcome.ValidateError(Errors.Project.NotFound);
    }

    private async Task AssertSuccessfulRequest(string email)
    {
        // Arrange
        var request = new CreateProjectSprintRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "Sprint 2 - Qualify on Semi-Finals",
            null,
            null);
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PostAsJsonAsync("api/projects/sprints", request);

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(2);
        var createdProject = await dbContext.ProjectSprints
            .Include(p => p.Project)
            .SingleAsync(p => p.Name.Value == request.Name);
        Utils.ProjectSprint.AssertCreation(createdProject, request);
        
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectResponse>();
        response!.Message.Should().Be("Project Sprint created successfully!");
    }
    
    private async Task AssertForbiddenResponse(string email)
    {
        // Arrange
        var request = new CreateProjectSprintRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "Sprint 2",
            null,
            null);
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PostAsJsonAsync("api/projects/sprints", request);

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(1);
        dbContext.ProjectSprints.SingleOrDefault(p => p.Name.Value == request.Name).Should().BeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}