using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Get;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerGetTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task GetCustomFieldSetup_WhenSuccessful_ShouldReturnCustomFieldSetup()
    {
        // Arrange
        var setup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateGetCustomFieldSetupRequest(setup.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Get($"api/customFieldSetups/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCustomFieldSetupResponse>(JsonOptions);
        Utils.CustomFieldSetup.AssertGetResponse(response!, setup);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCustomFieldSetupRequest();

        // Act
        var outcome = await Client.Get($"api/customFieldSetups/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.NotFound);
    }
}