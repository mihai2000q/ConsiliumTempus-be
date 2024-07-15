using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectSprint;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.AddStage;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerAddStageTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task AddStageToProjectSprint_WhenSucceeds_ShouldAddStageAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectSprintData.Users.First();
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateAddStageToProjectSprintRequest(sprint.Id.Value);
        
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/projects/sprints/Add-Stage", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, request, sprint, user);
    }

    [Fact]
    public async Task AddStageToProjectSprint_WhenRequestHasOnTop_ShouldAddStageAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectSprintData.Users.First();
        var sprint = ProjectSprintData.ProjectSprints.First();
        var request = ProjectSprintRequestFactory.CreateAddStageToProjectSprintRequest(
            sprint.Id.Value,
            onTop: true);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/projects/sprints/Add-Stage", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        await AssertSuccess(outcome, request, sprint, user);
    }

    [Fact]
    public async Task AddStageToProjectSprint_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateAddStageToProjectSprintRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/projects/sprints/Add-Stage", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectSprint.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectSprints.SingleOrDefault(p => p.Id == ProjectSprintId.Create(request.Id))
            .Should().BeNull();
    }

    private async Task AssertSuccess(
        HttpResponseMessage outcome,
        AddStageToProjectSprintRequest request,
        ProjectSprintAggregate sprint,
        UserAggregate createdBy)
    {
        var response = await outcome.Content.ReadFromJsonAsync<AddStageToProjectSprintResponse>();
        response!.Message.Should().Be("Stage has been successfully added to Project Sprint!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<ProjectStage>().Should().HaveCount(ProjectSprintData.ProjectStages.Length + 1);
        var newSprint = await dbContext.ProjectSprints
            .AsNoTracking()
            .Include(ps => ps.Stages.OrderBy(s => s.CustomOrderPosition.Value))
            .Include(ps => ps.Project.Workspace)
            .SingleAsync(ps => ps.Id == sprint.Id);
        Utils.ProjectSprint.AssertAddedStage(newSprint, request, createdBy);
    }
}