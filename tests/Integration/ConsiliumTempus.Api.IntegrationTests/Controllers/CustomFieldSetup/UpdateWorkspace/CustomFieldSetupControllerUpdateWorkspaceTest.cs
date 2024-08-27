using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.UpdateWorkspace;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerUpdateWorkspaceTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task
        UpdateWorkspaceCustomFieldSetup_WhenRequestHasNumberType_ShouldUpdateWorkspaceOnCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateUpdateWorkspaceCustomFieldSetupRequest(
            customFieldSetup.Id.Value,
            workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/customFieldSetups/Workspace", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateWorkspaceCustomFieldSetupResponse>();
        response!.Message.Should().Be("Custom Field Setup's Workspace has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedCustomFieldSetup = await dbContext.CustomFieldSetups
            .AsNoTracking()
            .Include(cfs => cfs.Audit)
            .Include(cfs => cfs.Workspace)
            .SingleAsync(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id));

        Utils.CustomFieldSetup.AssertUpdateWorkspace(request, updatedCustomFieldSetup, user);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenWorkspaceIsNotFound_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateUpdateWorkspaceCustomFieldSetupRequest(
            customFieldSetup.Id.Value,
            Guid.NewGuid());

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Put("api/customFieldSetups/Workspace", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.SingleOrDefault(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id))
            .Should().NotBeNull();
        dbContext.Workspaces.SingleOrDefault(w => w.Id == WorkspaceId.Create(request.WorkspaceId))
            .Should().BeNull();
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateUpdateWorkspaceCustomFieldSetupRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/customFieldSetups/Workspace", request);

        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.SingleOrDefault(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id))
            .Should().BeNull();
    }
}