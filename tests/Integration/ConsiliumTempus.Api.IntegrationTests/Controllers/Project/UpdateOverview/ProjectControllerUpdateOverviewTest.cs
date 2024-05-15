using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateOverview;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateOverviewTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateOverviewProject_WhenSucceeds_ShouldUpdateOverviewAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdateOverviewProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Put("api/projects/overview", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateOverviewProjectResponse>();
        response!.Message.Should().Be("Project Overview has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertUpdateOverview(project, updatedProject, request);
    }

    [Fact]
    public async Task UpdateOverviewProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOverviewProjectRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/overview", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id)).Should().BeNull();
    }
}