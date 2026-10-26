using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.User;
using ErrorOr;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.MakeGlobal;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerMakeGlobalAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    // Permission Authorization
    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0]);
    }

    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[3]);
    }

    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[4]);
    }

    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenWithoutMembership_ShouldReturnNotFoundError()
    {
        await AssertNotFoundResponse(Errors.CustomFieldSetup.NotFound, CustomFieldSetupData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[4], CustomFieldSetupData.CustomFieldSetups[^3]);
    }

    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^2]);
    }

    [Fact]
    public async Task MakeCustomFieldSetupGlobal_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnNotFoundError()
    {
        await AssertNotFoundResponse(Errors.CustomFieldSetup.NotFound, CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^1]);
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

    private async Task AssertNotFoundResponse(Error error, UserAggregate user, CustomFieldSetupAggregate? customFieldSetup = null)
    {
        var outcome = await ArrangeAndAct(user, customFieldSetup);

        // Assert
        await outcome.ValidateError(error);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(
        UserAggregate user,
        CustomFieldSetupAggregate? customFieldSetup = null)
    {
        // Arrange
        customFieldSetup ??= CustomFieldSetupData.CustomFieldSetups[1];
        var request = CustomFieldSetupRequestFactory.CreateMakeCustomFieldSetupGlobalRequest(customFieldSetup.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/customFieldSetups/Global", request);
    }
}