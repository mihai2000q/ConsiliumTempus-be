using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.RemoveStatus;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.RemoveStatus;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerRemoveStatusTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task RemoveStatusFromProject_WhenSucceeds_ShouldReturnStatusesFromProject()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var status = project.Statuses[0];
        var request = ProjectRequestFactory.CreateRemoveStatusFromProjectRequest(
            project.Id.Value,
            status.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete("api/projects/" +
                                          $"{request.Id}/Remove-Status/{request.StatusId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<RemoveStatusFromProjectResponse>();
        response!.Message.Should().Be("Status has been successfully removed from Project!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SelectMany(p => p.Statuses).Should().HaveCount(ProjectData.Statuses.Length - 1);
        var newProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .Include(p => p.Statuses)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertRemoveStatus(newProject, request);
    }

    [Fact]
    public async Task RemoveStatusFromProject_WhenStatusIsNotFound_ShouldReturnStatusNotFoundError()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateRemoveStatusFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete("api/projects/" +
                                          $"{request.Id}/Remove-Status/{request.StatusId}");

        // Assert
        await outcome.ValidateError(Errors.ProjectStatus.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SelectMany(p => p.Statuses).Should().HaveCount(ProjectData.Statuses.Length);
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Projects.SelectMany(p => p.Statuses)
            .SingleOrDefault(ps => ps.Id == ProjectStatusId.Create(request.StatusId))
            .Should().BeNull();
    }

    [Fact]
    public async Task RemoveStatusFromProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveStatusFromProjectRequest();

        // Act
        var outcome = await Client.Delete("api/projects/" +
                                          $"{request.Id}/Remove-Status/{request.StatusId}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SelectMany(p => p.Statuses).Should().HaveCount(ProjectData.Statuses.Length);
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}