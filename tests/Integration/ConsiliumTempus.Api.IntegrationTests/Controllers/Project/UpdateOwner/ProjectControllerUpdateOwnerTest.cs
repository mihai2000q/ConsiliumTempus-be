using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.UpdateOwner;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateOwner;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateOwnerTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateOwnerProject_WhenSucceeds_ShouldUpdateIsPrivateAndReturnSuccessResponse()
    {
        // Arrange
        var collaborator = ProjectData.Users[3];
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdateOwnerProjectRequest(
            project.Id.Value, 
            collaborator.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Put("api/projects/Owner", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateOwnerProjectResponse>();
        response!.Message.Should().Be("Project owner has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertUpdateOwner(updatedProject, request);
    }

    [Fact]
    public async Task UpdateOwnerProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOwnerProjectRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/Owner", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}