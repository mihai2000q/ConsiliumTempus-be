using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.RemoveFromProject;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerRemoveFromProjectTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task
        RemoveCustomFieldSetupFromProject_WhenIsSuccessful_ShouldRemoveCustomFieldSetupFromProjectAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups[1];
        var project = customFieldSetup.Projects[0];
        var request = CustomFieldSetupRequestFactory.CreateRemoveCustomFieldSetupFromProjectRequest(
            customFieldSetup.Id.Value,
            project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete("api/customFieldSetups/" +
                                          $"{request.Id}/Remove-Project/{request.ProjectId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateWorkspaceCustomFieldSetupResponse>();
        response!.Message.Should().Be("Custom Field Setup has been successfully removed from project!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedCustomFieldSetup = await dbContext.CustomFieldSetups
            .AsNoTracking()
            .Include(cfs => cfs.Audit)
            .Include(cfs => cfs.Projects)
            .Include(cfs => cfs.Workspace)
            .SingleAsync(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id));

        var updatedProject = await dbContext.Projects
            .Include(p => p.Sprints)
            .ThenInclude(ps => ps.Stages)
            .ThenInclude(ps => ps.Tasks)
            .ThenInclude(pt => pt.CustomFields)
            .SingleAsync(p => p.Id == ProjectId.Create(request.ProjectId));

        Utils.CustomFieldSetup.AssertRemoveFromProject(request, updatedCustomFieldSetup, updatedProject, user);
    }

    [Fact]
    public async Task RemoveCustomFieldSetupFromProject_WhenProjectIsNotFound_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups[1];
        var request = CustomFieldSetupRequestFactory.CreateAddCustomFieldSetupToProjectRequest(
            customFieldSetup.Id.Value,
            Guid.NewGuid());

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Delete("api/customFieldSetups/" +
                                          $"{request.Id}/Remove-Project/{request.ProjectId}");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.SingleOrDefault(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.ProjectId))
            .Should().BeNull();
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenIsNotGlobal_ShouldReturnNotGlobalError()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateAddCustomFieldSetupToProjectRequest(
            customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete("api/customFieldSetups/" +
                                          $"{request.Id}/Remove-Project/{request.ProjectId}");
        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.NotGlobal);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.SingleOrDefault(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id))
            .Should().NotBeNull();
        customFieldSetup.Workspace.Should().BeNull();
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateAddCustomFieldSetupToProjectRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Delete("api/customFieldSetups/" +
                                          $"{request.Id}/Remove-Project/{request.ProjectId}");
        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.SingleOrDefault(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id))
            .Should().BeNull();
    }
}