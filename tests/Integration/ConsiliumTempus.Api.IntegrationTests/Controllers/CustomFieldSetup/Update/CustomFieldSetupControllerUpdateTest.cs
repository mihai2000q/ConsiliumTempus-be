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
    public async Task UpdateCustomFieldSetup_WhenRequestHasDateType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[1];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.Date,
            dateCustomFieldSetup: new UpdateCustomFieldSetupRequest.DateCustomFieldSetupRequest(
                new DateOnly(2022, 10, 10)));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestHasDateTimeType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[2];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.DateTime,
            dateTimeCustomFieldSetup: new UpdateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(
                new DateTime(2022, 10, 10, 10, 55, 30)));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestHasDurationType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[3];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.Duration,
            durationCustomFieldSetup: new UpdateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(
                new TimeSpan(24, 55, 30)));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasMultiSelectType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[4];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.MultiSelect,
            multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                null,
                null,
                null,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasMultiSelectTypeAndAddOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[4];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.MultiSelect,
            multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                UpdateCustomFieldSetupRequest.OptionOperation.Add,
                new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                    "#2233FF",
                    "Medium"),
                null,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasMultiSelectTypeAndUpdateOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[4];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.MultiSelect,
            multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                UpdateCustomFieldSetupRequest.OptionOperation.Update,
                new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest(
                    "#FF0000",
                    "High-Priority"),
                ((MultiSelectCustomFieldSetupAggregate)setup).Options[0].Id,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasMultiSelectTypeAndMoveOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[4];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.MultiSelect,
            multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                UpdateCustomFieldSetupRequest.OptionOperation.Move,
                null,
                ((MultiSelectCustomFieldSetupAggregate)setup).Options[1].Id,
                ((MultiSelectCustomFieldSetupAggregate)setup).Options[0].Id));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasMultiSelectTypeAndRemoveOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[4];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.MultiSelect,
            multiSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                UpdateCustomFieldSetupRequest.OptionOperation.Remove,
                null,
                ((MultiSelectCustomFieldSetupAggregate)setup).Options[1].Id,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestHasNumberType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[5];
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
    public async Task UpdateCustomFieldSetup_WhenRequestHasPeopleType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[6];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.People);

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[7];
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
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndAddOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[7];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                null,
                UpdateCustomFieldSetupRequest.OptionOperation.Add,
                new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                    "#2233FF",
                    "Medium"),
                null,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndUpdateOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[7];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[0].Id,
                UpdateCustomFieldSetupRequest.OptionOperation.Update,
                new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest(
                    "#FF0000",
                    "High-Priority"),
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[0].Id,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndMoveOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[7];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                null,
                UpdateCustomFieldSetupRequest.OptionOperation.Move,
                null,
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[1].Id,
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[0].Id));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        UpdateCustomFieldSetup_WhenRequestHasSingleSelectTypeAndRemoveOperation_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[7];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                null,
                UpdateCustomFieldSetupRequest.OptionOperation.Remove,
                null,
                ((SingleSelectCustomFieldSetupAggregate)setup).Options[1].Id,
                null));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestHasTextType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[8];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.Text,
            textCustomFieldSetup: new UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest("Default"));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestHasTimeType_ShouldUpdateSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var setup = CustomFieldSetupData.CustomFieldSetups[9];
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.Time,
            timeCustomFieldSetup: new UpdateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(
                new TimeOnly(20, 50)));

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