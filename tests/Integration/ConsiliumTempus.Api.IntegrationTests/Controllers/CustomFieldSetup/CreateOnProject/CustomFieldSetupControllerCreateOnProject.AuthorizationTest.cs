using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.CreateOnProject;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerCreateOnProjectAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    // Permission Authorization
    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0]);
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[3]);
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[4]);
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[4], CustomFieldSetupData.Projects[^3]);
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.Projects[^2]);
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.Projects[^1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user, ProjectAggregate? project = null)
    {
        var outcome = await ArrangeAndAct(user, project);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user, ProjectAggregate? project = null)
    {
        var outcome = await ArrangeAndAct(user, project);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user, ProjectAggregate? project = null)
    {
        // Arrange
        project ??= CustomFieldSetupData.Projects.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
            project.Id.Value,
            textCustomFieldSetup: new CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest(null));

        // Act
        Client.UseCustomToken(user);
        return await Client.Post($"api/customFieldSetups/Project", request);
    }
}