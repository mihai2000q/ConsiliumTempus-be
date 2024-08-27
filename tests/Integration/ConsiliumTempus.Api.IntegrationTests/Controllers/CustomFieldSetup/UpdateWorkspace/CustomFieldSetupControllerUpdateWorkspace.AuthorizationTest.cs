using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.UpdateWorkspace;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerUpdateWorkspaceAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    // Permission Authorization
    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0]);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[3]);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[4]);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[4], CustomFieldSetupData.CustomFieldSetups[^3]);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.CustomFieldSetups[^2]);
    }

    [Fact]
    public async Task UpdateWorkspaceCustomFieldSetup_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
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
        var workspace = CustomFieldSetupData.Workspaces[0];
        var request = CustomFieldSetupRequestFactory.CreateUpdateWorkspaceCustomFieldSetupRequest(
            customFieldSetup.Id.Value,
            workspace.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/customFieldSetups/Workspace", request);
    }
}