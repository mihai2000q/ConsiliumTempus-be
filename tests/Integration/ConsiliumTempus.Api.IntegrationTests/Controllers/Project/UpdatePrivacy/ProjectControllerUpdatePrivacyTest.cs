using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.UpdatePrivacy;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdatePrivacy;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdatePrivacyTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdatePrivacyProject_WhenSucceeds_ShouldUpdateIsPrivateAndReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdatePrivacyProjectRequest(
            project.Id.Value, 
            true);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Put("api/projects/privacy", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdatePrivacyProjectResponse>();
        response!.Message.Should().Be("Project privacy has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertUpdatePrivacy(updatedProject, request);
    }

    [Fact]
    public async Task UpdatePrivacyProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdatePrivacyProjectRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/privacy", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}