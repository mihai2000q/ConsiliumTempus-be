using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Create;

public class ProjectControllerCreateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Project")
{
    [Fact]
    public async Task WhenProjectCreateSucceeds_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var request = new CreateProjectRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "Project Name",
            "This is the project description",
            true);

        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.PostAsJsonAsync("api/projects", request);

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(2);
        var createdProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .Include(p => p.Sprints)
            .ThenInclude(ps => ps.Sections.OrderBy(s => s.Order.Value))
            .ThenInclude(ps => ps.Tasks.OrderBy(t => t.Order.Value))
            .SingleAsync(p => p.Name.Value == request.Name);
        Utils.Project.AssertCreation(createdProject, request);

        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectResponse>();
        response!.Message.Should().Be("Project created successfully!");
    }

    [Fact]
    public async Task WhenProjectCreateFails_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var request = new CreateProjectRequest(
            new Guid("90000000-0000-0000-0000-000000000000"),
            "Project Name",
            "This is the project description",
            true);

        // Act
        var outcome = await Client.PostAsJsonAsync("api/projects", request);

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(1);
        dbContext.Projects.SingleOrDefault(p => p.Name.Value == request.Name).Should().BeNull();

        await outcome.ValidateError(Errors.Workspace.NotFound);
    }
}