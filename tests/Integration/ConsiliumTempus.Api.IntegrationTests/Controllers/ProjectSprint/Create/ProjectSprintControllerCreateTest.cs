using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Create;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerCreateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task CreateProjectSprint_WhenSucceeds_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectSprintData.Users.First();
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(project.Id.Value);

        await ActAndAssertSuccess(user, project, request);
    }

    [Fact]
    public async Task CreateProjectSprint_WhenRequestHasKeepPreviousStages_ShouldCreateKeepStagesAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectSprintData.Users[1];
        var project = ProjectSprintData.Projects[1];
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(
            project.Id.Value,
            keepPreviousStages: true);

        await ActAndAssertSuccess(user, project, request);
    }

    [Fact]
    public async Task CreateProjectSprint_WhenRequestHasKeepPreviousStagesAndIsFirstSprint_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectSprintData.Users.First();
        var project = ProjectSprintData.Projects[2];
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(
            project.Id.Value,
            keepPreviousStages: true);

        await ActAndAssertSuccess(user, project, request);
        project.Sprints.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateProjectSprint_WhenRequestHasProjectStatus_ShouldCreateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectSprintData.Users.First();
        var project = ProjectSprintData.Projects.First();
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(
            project.Id.Value,
            projectStatus: ProjectSprintRequestFactory.CreateCreateProjectStatus());

        await ActAndAssertSuccess(user, project, request);
    }

    [Fact]
    public async Task CreateProjectSprint_WhenProjectIsNotFound_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(ProjectSprintData.ProjectSprints.Length);
        dbContext.ProjectSprints.SingleOrDefault(p => p.Name.Value == request.Name)
            .Should().BeNull();
    }

    private async Task ActAndAssertSuccess(
        UserAggregate user,
        ProjectAggregate project,
        CreateProjectSprintRequest request)
    {
        var previousSprintEndDate = project.Sprints
            .IfNotEmpty(sprints => sprints[0].EndDate);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectSprintResponse>();
        response!.Message.Should().Be("Project Sprint has been created successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.Should().HaveCount(ProjectSprintData.ProjectSprints.Length + 1);
        var createdSprint = await dbContext.ProjectSprints
            .Include(ps => ps.Audit)
            .Include(ps => ps.Project.Workspace)
            .Include(ps => ps.Project.Sprints)
            .Include(ps => ps.Project.Statuses)
            .Include(ps => ps.Stages)
            .SingleAsync(ps => ps.Name.Value == request.Name);
        Utils.ProjectSprint.AssertCreation(
            createdSprint,
            request,
            project,
            user,
            previousSprintEndDate);
    }
}