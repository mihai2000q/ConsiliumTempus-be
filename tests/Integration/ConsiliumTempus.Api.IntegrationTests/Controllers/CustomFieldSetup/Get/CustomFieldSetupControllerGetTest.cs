using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Get;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerGetTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    private static readonly Dictionary<Type, Type> SetupTypeToResponseType = new()
    {
        { typeof(DateCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetDateCustomFieldSetupResponse) },
        { typeof(DateTimeCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetDateTimeCustomFieldSetupResponse) },
        { typeof(DurationCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetDurationCustomFieldSetupResponse) },
        { typeof(MultiSelectCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetMultiSelectCustomFieldSetupResponse) },
        { typeof(NumberCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetNumberCustomFieldSetupResponse) },
        { typeof(PeopleCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetPeopleCustomFieldSetupResponse) },
        { typeof(SingleSelectCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetSingleSelectCustomFieldSetupResponse) },
        { typeof(TextCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetTextCustomFieldSetupResponse) },
        { typeof(TimeCustomFieldSetupAggregate), typeof(GetCustomFieldSetupResponse.GetTimeCustomFieldSetupResponse) }
    };

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
        var response = await outcome.Content.ReadFromJsonAsync(SetupTypeToResponseType[setup.GetType()], JsonOptions);
        Utils.CustomFieldSetup.AssertGetResponse((GetCustomFieldSetupResponse)response!, setup);
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