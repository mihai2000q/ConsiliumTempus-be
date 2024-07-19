using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Update;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateProject_WhenSucceeds_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdateProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateProjectResponse>();
        response!.Message.Should().Be("Project has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertUpdate(project, updatedProject, request);
    }

    [Fact]
    public async Task UpdateProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateProjectRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id)).Should().BeNull();
    }
}