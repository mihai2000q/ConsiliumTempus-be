using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.AddStatus;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.AddStatus;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerAddStatusTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task AddStatusToProject_WhenSucceeds_ShouldAddStatusToProjectAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateAddStatusToProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/projects/Add-Status", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<AddStatusToProjectResponse>();
        response!.Message.Should().Be("Status has been successfully added to Project!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SelectMany(p => p.Statuses).Should().HaveCount(ProjectData.Statuses.Length + 1);
        var newProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .Include(p => p.Statuses)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertAddStatus(newProject, request, user);
    }

    [Fact]
    public async Task AddStatusToProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddStatusToProjectRequest();

        // Act
        var outcome = await Client.Post("api/projects/Add-Status", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SelectMany(p => p.Statuses).Should().HaveCount(ProjectData.Statuses.Length);
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
        dbContext.Projects.SelectMany(p => p.Statuses)
            .SingleOrDefault(p => p.Title.Value == request.Title)
            .Should().BeNull();
    }
}