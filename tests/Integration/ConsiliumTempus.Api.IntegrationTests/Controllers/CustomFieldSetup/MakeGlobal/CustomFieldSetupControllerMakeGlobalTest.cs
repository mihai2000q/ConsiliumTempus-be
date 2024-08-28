using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.MakeGlobal;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.MakeGlobal;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerMakeGlobalTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task
        UpdateWorkspaceCustomFieldSetup_WhenRequestHasNumberType_ShouldUpdateWorkspaceOnCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest(customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/customFieldSetups/Global", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<MakeCustomFieldGlobalResponse>();
        response!.Message.Should().Be("Custom Field Setup has been made globally available in the workspace!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedCustomFieldSetup = await dbContext.CustomFieldSetups
            .AsNoTracking()
            .Include(cfs => cfs.Audit)
            .Include(cfs => cfs.Workspace)
            .SingleAsync(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id));

        Utils.CustomFieldSetup.AssertMakeGlobal(request, updatedCustomFieldSetup, user);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenItHasWorkspace_ShouldReturnAlreadyGlobalError()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups[1];
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest(customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Put("api/customFieldSetups/Global", request);

        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.AlreadyGlobal);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.SingleOrDefault(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id))
            .Should().NotBeNull();
        customFieldSetup.Workspace.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Put("api/customFieldSetups/Global", request);

        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.SingleOrDefault(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id))
            .Should().BeNull();
    }
}