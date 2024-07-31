using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.LeavePrivate;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.LeavePrivate;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerLeavePrivateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task LeavePrivateProject_WhenSucceeds_ShouldLeavePrivateAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects[^2];
        var user = project.AllowedMembers.First(u => u != project.Owner);
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/projects/{request.Id}/Leave");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<LeavePrivateProjectResponse>();
        response!.Message.Should().Be("Project has been left successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.AllowedMembers)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertLeavePrivate(updatedProject, request, user);
    }
    
    [Fact]
    public async Task LeavePrivateProject_WhenIsOwner_ShouldReturnLeaveOwnedError()
    {
        // Arrange
        var project = ProjectData.Projects[^2];
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Delete($"api/projects/{request.Id}/Leave");

        // Assert
        await outcome.ValidateError(Errors.Project.LeaveOwned);
    }
    
    [Fact]
    public async Task LeavePrivateProject_WhenIsNotPrivate_ShouldReturnNotPrivateError()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Delete($"api/projects/{request.Id}/Leave");

        // Assert
        await outcome.ValidateError(Errors.Project.NotPrivate);

        project.IsPrivate.Value.Should().BeFalse();
    }

    [Fact]
    public async Task LeavePrivateProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest();

        // Act
        var outcome = await Client.Delete($"api/projects/{request.Id}/Leave");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Should().HaveCount(ProjectData.Projects.Length);
        dbContext.Projects
            .SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}