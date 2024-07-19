using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.UpdateFavorites;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateFavorites;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateFavoritesTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateFavoritesProject_WhenSucceeds_ShouldUpdateFavoritesAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdateFavoritesProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/Favorites", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateFavoritesProjectResponse>();
        response!.Message.Should().Be("Project favorites have been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedProject = await dbContext.Projects
            .Include(p => p.Workspace)
            .SingleAsync(p => p.Id == ProjectId.Create(request.Id));
        Utils.Project.AssertUpdateFavorites(updatedProject, request, user);
    }

    [Fact]
    public async Task UpdateFavoritesProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateFavoritesProjectRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/Favorites", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.Id))
            .Should().BeNull();
    }
}