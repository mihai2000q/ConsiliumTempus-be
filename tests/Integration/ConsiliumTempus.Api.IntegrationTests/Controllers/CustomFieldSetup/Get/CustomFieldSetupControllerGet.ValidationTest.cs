using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Get;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerGetValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task GetCustomFieldSetup_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var setup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateGetCustomFieldSetupRequest(setup.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Get($"api/customFieldSetups/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCustomFieldSetupRequest(Guid.Empty);

        // Act
        var outcome = await Client.Get($"api/customFieldSetups/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}