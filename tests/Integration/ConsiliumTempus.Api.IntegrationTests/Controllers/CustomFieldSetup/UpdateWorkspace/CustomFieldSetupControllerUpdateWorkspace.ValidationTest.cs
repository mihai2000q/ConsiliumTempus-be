using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.UpdateWorkspace;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerUpdateWorkspaceValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateUpdateWorkspaceCustomFieldSetupRequest(
            customFieldSetup.Id.Value,
            workspace.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Put("api/customFieldSetups/Workspace", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateUpdateWorkspaceCustomFieldSetupRequest(
            id: Guid.Empty, 
            workspaceId: Guid.Empty);  

        // Act
        var outcome = await Client.Put("api/customFieldSetups/Workspace", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}