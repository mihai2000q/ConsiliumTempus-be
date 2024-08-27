using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;
using ConsiliumTempus.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Update;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerUpdateTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestHasNumberType_ShouldReturnUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[0];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.Number,
            numberCustomFieldSetup: new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                    "USD",
                    2,
                    false),
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectType_ShouldReturnUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[2];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                null,
                null,
                null,
                null,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndAddOperation_ShouldReturnUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[2];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                null,
                UpdateCustomFieldSetupRequest.SingleSelectOptionOperation.Add,
                new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                    "#2233FF",
                    "Medium"),
                null,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndUpdateOperation_ShouldReturnUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[2];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[0].Id,
                UpdateCustomFieldSetupRequest.SingleSelectOptionOperation.Update,
                new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                    "#FF0000",
                    "High-Priority"),
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[0].Id,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndMoveOperation_ShouldReturnUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[2];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                null,
                UpdateCustomFieldSetupRequest.SingleSelectOptionOperation.Move,
                null,
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[1].Id,
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[0].Id));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndRemoveOperation_ShouldReturnUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[2];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                null,
                UpdateCustomFieldSetupRequest.SingleSelectOptionOperation.Remove,
                null,
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[1].Id,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestHasTextType_ShouldReturnUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[3];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.Text,
            textCustomFieldSetup: new UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest("Default"));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            Guid.NewGuid(),
            type: CustomFieldType.Text,
            textCustomFieldSetup: new UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest(null));

        // Act
        var outcome = await Client.Put("api/customFieldSetups", request);

        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }

    private async Task ActAndAssert(
        UpdateCustomFieldSetupRequest request,
        UserAggregate user)
    {
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/customFieldSetups", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateCustomFieldSetupResponse>();
        response!.Message.Should().Be("Custom Field Setup has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedSetup = dbContext.CustomFieldSetups
            .Include(cfs => cfs.Workspace)
            .Include(cfs => cfs.Projects)
            .Single(cfs => cfs.Id == CustomFieldSetupId.Create(request.Id));

        Utils.CustomFieldSetup.AssertUpdate(request, updatedSetup, user);
    }
}