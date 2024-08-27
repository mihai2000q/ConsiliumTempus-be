using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Update;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerUpdateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var setup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            setup.Id.Value,
            type: CustomFieldType.Number,
            numberCustomFieldSetup: new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                    "USD",
                    2,
                    false),
                null));

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Put("api/customFieldSetups", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(Guid.Empty);

        // Act
        var outcome = await Client.Put("api/customFieldSetups", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}