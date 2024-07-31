using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdatePrivacy;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdatePrivacyProjectAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    // Workspace Authorization
    [Fact]
    public async Task UpdatePrivacyProject_WhenIsCollaborator_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[0]);
    }

    [Fact]
    public async Task UpdatePrivacyProject_WhenIsNotCollaborator_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[1], ProjectData.Projects[^4]);
    }

    // Project Authorization
    [Fact]
    public async Task UpdatePrivacyProject_WhenIsOwner_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[0]);
    }

    [Fact]
    public async Task UpdatePrivacyProject_WhenIsNotOwner_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectData.Users[3]);
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
        project ??= ProjectData.Projects[^2];
        var request = ProjectRequestFactory.CreateUpdatePrivacyProjectRequest(
            project.Id.Value,
            true);

        // Act
        Client.UseCustomToken(user);
        return await Client.Put("api/projects/privacy", request);
    }
}