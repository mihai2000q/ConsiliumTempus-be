using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.AddToProject;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerAddToProjectValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task AddCustomFieldSetupToProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var project = CustomFieldSetupData.Projects.First();
        var request = CustomFieldSetupRequestFactory.CreateAddCustomFieldSetupToProjectRequest(
            customFieldSetup.Id.Value,
            project.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Post("api/customFieldSetups/Add-Project", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddCustomFieldSetupToProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateAddCustomFieldSetupToProjectRequest(
            id: Guid.Empty,
            projectId: Guid.Empty);

        // Act
        var outcome = await Client.Post("api/customFieldSetups/Add-Project", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}