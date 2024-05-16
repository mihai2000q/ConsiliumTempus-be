using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Update;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerUpdateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task UpdateProjectSprint_WhenSuccessful_ShouldUpdateAndReturnSuccessfulResponse()
    {
        // Arrange
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateUpdateProjectSprintRequest(
            sprint.Id.Value);

        // Act
        Client.UseCustomToken(sprint.Project.Workspace.Memberships[0].User);
        var outcome = await Client.Put("api/projects/sprints", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateProjectSprintResponse>();
        response!.Message.Should().Be("Project Sprint has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var newSprint = dbContext.ProjectSprints
            .Include(ps => ps.Project.Workspace)
            .Single(ps => ps.Id == ProjectSprintId.Create(request.Id));
        
        Utils.ProjectSprint.AssertUpdated(
            sprint,
            newSprint,
            request);
    }

    [Fact]
    public async Task UpdateProjectSprint_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateUpdateProjectSprintRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/projects/sprints", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);
    }
}