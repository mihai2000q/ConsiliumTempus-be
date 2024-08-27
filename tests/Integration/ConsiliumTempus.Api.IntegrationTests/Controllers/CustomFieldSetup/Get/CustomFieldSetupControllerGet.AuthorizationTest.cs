using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Get;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerGetAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    // Permission Authorization
    [Fact]
    public async Task GetCustomFieldSetup_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[3]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[4]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task GetCustomFieldSetup_WhenIsGlobal_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^4]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[4], CustomFieldSetupData.CustomFieldSetups[^3]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^2]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user, CustomFieldSetupAggregate? customFieldSetup = null)
    {
        var outcome = await ArrangeAndAct(user, customFieldSetup);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user, CustomFieldSetupAggregate? customFieldSetup = null)
    {
        var outcome = await ArrangeAndAct(user, customFieldSetup);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(
        UserAggregate user,
        CustomFieldSetupAggregate? customFieldSetup = null)
    {
        // Arrange
        customFieldSetup ??= CustomFieldSetupData.CustomFieldSetups.First();
        var request = CustomFieldSetupRequestFactory.CreateGetCustomFieldSetupRequest(customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Get($"api/customFieldSetups/{request.Id}");
    }
}