using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.Update;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerUpdateAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    // Permission Authorization
    [Fact]
    public async Task UpdateCustomFieldSetup_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0]);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[3]);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[4]);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task UpdateCustomFieldSetup_WhenIsGlobal_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^4]);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[4], CustomFieldSetupData.CustomFieldSetups[^3]);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^2]);
    }

    [Fact]
    public async Task UpdateCustomFieldSetup_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
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
        var request = CustomFieldSetupRequestFactory.CreateUpdateCustomFieldSetupRequest(
            customFieldSetup.Id.Value,
            type: CustomFieldType.Number,
            numberCustomFieldSetup: new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                new UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                    null,
                    2,
                    false),
                null));

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/customFieldSetups/", request);
    }
}