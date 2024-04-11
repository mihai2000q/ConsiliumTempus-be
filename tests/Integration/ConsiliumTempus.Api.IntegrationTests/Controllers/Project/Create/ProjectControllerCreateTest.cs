using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Create;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerCreateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task WhenProjectCreateSucceeds_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var workspaceId = ProjectData.Workspaces.First().Id.Value;
        var request = ProjectRequestFactory.CreateCreateProjectRequest(workspaceId);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Post("api/projects", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectResponse>();
        response!.Message.Should().Be("Project created successfully!");
        
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(ProjectData.Projects.Length + 1);
        var createdProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .Include(p => p.Sprints)
            .ThenInclude(ps => ps.Sections.OrderBy(s => s.Order.Value))
            .ThenInclude(ps => ps.Tasks.OrderBy(t => t.Order.Value))
            .SingleAsync(p => p.Name.Value == request.Name);
        Utils.Project.AssertCreation(createdProject, request);
    }

    [Fact]
    public async Task CreateProject_WhenUserIsNotFound_ShouldReturnForbiddenResponse()
    {
        // Arrange
        var workspaceId = ProjectData.Workspaces.First().Id.Value;
        var request = ProjectRequestFactory.CreateCreateProjectRequest(workspaceId);

        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Post("api/projects", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateProject_WhenWorkspaceIsNotFound_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/projects", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
        
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(ProjectData.Projects.Length);
        dbContext.Projects.SingleOrDefault(p => p.Name.Value == request.Name).Should().BeNull();
    }
}