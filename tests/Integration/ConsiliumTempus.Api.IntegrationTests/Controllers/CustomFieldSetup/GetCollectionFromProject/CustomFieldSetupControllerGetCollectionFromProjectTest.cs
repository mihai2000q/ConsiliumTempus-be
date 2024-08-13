using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.GetCollectionFromProject;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerGetCollectionFromProjectTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task GetCollectionCustomFieldSetupFromProject_WhenSuccessful_ShouldReturnCustomFieldSetups()
    {
        // Arrange
        var project = CustomFieldSetupData.Projects.First();
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromProjectRequest(
            project.Id.Value);

        // Act
        Client.UseCustomToken(CustomFieldSetupData.Users.First());
        var outcome = await Client.Get($"api/customFieldSetups/project/{request.ProjectId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionCustomFieldSetupFromProjectResponse>();
        Utils.CustomFieldSetup.AssertGetCollectionFromProjectResponse(
            response!,
            project.CustomFieldSetups);
    }

    [Fact]
    public async Task GetCollectionCustomFieldSetupFromProject_WhenProjectIsNotFound_ShouldReturnEmptyCustomFieldSetups()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromProjectRequest(
            Guid.NewGuid());

        // Act
        var outcome = await Client.Get($"api/customFieldSetups/project/{request.ProjectId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCollectionCustomFieldSetupFromProjectResponse>();
        response!.CustomFieldSetups.Should().BeEmpty();
    }
}