using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.UpdateStatus;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateStatus;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateStatusTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateStatusFromProject_WhenSucceeds_ShouldReturnStatusesFromProject()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var status = project.Statuses[0];
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest(
            project.Id.Value,
            status.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/Update-Status", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateStatusFromProjectResponse>();
        response!.Message.Should().Be("Status has been successfully updated from Project!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var newProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .Include(p => p.Statuses)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertUpdateStatus(newProject, request, user);
    }

    [Fact]
    public async Task UpdateStatusFromProject_WhenStatusIsNotFound_ShouldReturnStatusNotFoundError()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/Update-Status", request);

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
    public async Task UpdateStatusFromProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest();

        // Act
        var outcome = await Client.Put("api/projects/Update-Status", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SelectMany(p => p.Statuses).Should().HaveCount(ProjectData.Statuses.Length);
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}