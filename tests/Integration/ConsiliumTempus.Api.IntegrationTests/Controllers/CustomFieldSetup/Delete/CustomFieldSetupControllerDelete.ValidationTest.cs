using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Delete;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerDeleteValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task DeleteCustomFieldSetup_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateDeleteCustomFieldSetupRequest(customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Delete($"api/customFieldSetups/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteCustomFieldSetup_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateDeleteCustomFieldSetupRequest(Guid.Empty);

        // Act
        var outcome = await Client.Delete($"api/customFieldSetups/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}