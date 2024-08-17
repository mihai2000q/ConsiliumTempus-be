using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup.ValueObjects;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Delete;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerDeleteTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task DeleteCustomFieldSetup_WhenSuccessful_ShouldDeleteCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var customFieldSetup = CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateDeleteCustomFieldSetupRequest(customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/customFieldSetups/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteCustomFieldSetupResponse>();
        response!.Message.Should().Be("Custom Field Setup has been deleted successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.Should().HaveCount(CustomFieldSetupData.CustomFieldSetups.Length - 1);
        (await dbContext.CustomFieldSetups.FindAsync(customFieldSetup.Id))
            .Should().BeNull();
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateDeleteCustomFieldSetupRequest();

        // Act
        var outcome = await Client.Delete($"api/customFieldSetups/{request.Id}");

        // Assert
        await outcome.ValidateError(Errors.CustomFieldSetup.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.Should().HaveCount(CustomFieldSetupData.CustomFieldSetups.Length);
        (await dbContext.CustomFieldSetups.FindAsync(CustomFieldSetupId.Create(request.Id)))
            .Should().BeNull();
    }
}