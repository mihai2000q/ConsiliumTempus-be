using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.RemoveFromProject;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerRemoveFromProjectValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task RemoveCustomFieldSetupFromProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups[1];
        var project = customFieldSetup.Projects[0];
        var request = CustomFieldSetupRequestFactory.CreateRemoveCustomFieldSetupFromProjectRequest(
            customFieldSetup.Id.Value,
            project.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Delete("api/customFieldSetups/" +
                                          $"{request.Id}/Remove-Project/{request.ProjectId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RemoveCustomFieldSetupFromProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateRemoveCustomFieldSetupFromProjectRequest(
            id: Guid.Empty,
            projectId: Guid.Empty);

        // Act
        var outcome = await Client.Delete("api/customFieldSetups/" +
                                          $"{request.Id}/Remove-Project/{request.ProjectId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}