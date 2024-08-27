using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.GetCollectionFromProject;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerGetCollectionFromProjectAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    // Permission Authorization
    [Fact]
    public async Task GetCollectionCustomFieldSetupFromProject_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0]);
    }

    [Fact]
    public async Task GetCollectionCustomFieldSetupFromProject_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[3]);
    }

    [Fact]
    public async Task GetCollectionCustomFieldSetupFromProject_WhenWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[4]);
    }

    [Fact]
    public async Task GetCollectionCustomFieldSetupFromProject_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(CustomFieldSetupData.Users[1]);
    }

    // Project Authorization
    [Fact]
    public async Task GetCustomFieldSetup_WhenProjectIsNotPrivate_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[4], CustomFieldSetupData.Projects[^3]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenProjectIsPrivateAndIsAllowedMember_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(CustomFieldSetupData.Users[0], CustomFieldSetupData.Projects[^2]);
    }

    [Fact]
    public async Task GetCustomFieldSetup_WhenProjectIsPrivateButIsNotAllowedMember_ShouldReturnForbiddenResponse()
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
        var request = CustomFieldSetupRequestFactory.CreateGetCollectionCustomFieldSetupFromProjectRequest(
            project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Get($"api/customFieldSetups/Project/{request.ProjectId}");
    }
}