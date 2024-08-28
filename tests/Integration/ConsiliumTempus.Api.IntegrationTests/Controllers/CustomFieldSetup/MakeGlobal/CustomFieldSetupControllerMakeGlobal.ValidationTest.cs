using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.MakeGlobal;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerUpdateWorkspaceValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest(customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Put("api/customFieldSetups/Global", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest(id: Guid.Empty);  

        // Act
        var outcome = await Client.Put("api/customFieldSetups/Global", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}