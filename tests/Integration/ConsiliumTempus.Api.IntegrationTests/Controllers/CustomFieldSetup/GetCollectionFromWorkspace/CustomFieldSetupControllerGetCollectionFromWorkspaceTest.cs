using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.GetCollectionFromWorkspace;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerGetCollectionFromWorkspaceTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task GetCollectionCustomFieldSetupFromWorkspace_WhenSuccessful_ShouldReturnCustomFieldSetups()
    {
        // Arrange
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromWorkspaceRequest(
            workspace.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Get($"api/customFieldSetups/Workspace/{request.WorkspaceId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content
            .ReadFromJsonAsync<GetCollectionCustomFieldSetupFromWorkspaceResponse>(JsonOptions);
        Utils.CustomFieldSetup.AssertGetCollectionFromWorkspaceResponse(
            response!,
            workspace.CustomFieldSetups);
    }

    [Fact]
    public async Task
        GetCollectionCustomFieldSetupFromWorkspace_WhenWorkspaceIsNotFound_ShouldReturnEmptyCustomFieldSetups()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromWorkspaceRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/customFieldSetups/Workspace/{request.WorkspaceId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionCustomFieldSetupFromWorkspaceResponse>();
        response!.CustomFieldSetups.Should().BeEmpty();
    }
}