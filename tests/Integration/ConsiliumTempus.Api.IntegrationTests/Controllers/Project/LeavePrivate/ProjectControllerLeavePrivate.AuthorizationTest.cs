using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ErrorOr;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.LeavePrivate;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerLeavePrivateAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    // Workspace Authorization
    [Fact]
    public async Task LeavePrivateProject_WhenIsCollaborator_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[3]);
    }

    [Fact]
    public async Task LeavePrivateProject_WhenIsNotCollaborator_ShouldReturnNotFoundError()
    {
        await AssertNotFoundResponse(Errors.Project.NotFound, ProjectData.Users[1], ProjectData.Projects[^4]);
    }

    // Project Authorization
    [Fact]
    public async Task LeavePrivateProject_WhenIsAllowed_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectData.Users[3]);
    }

    [Fact]
    public async Task LeavePrivateProject_WhenIsNotAllowed_ShouldReturnNotFoundError()
    {
        await AssertNotFoundResponse(Errors.Project.NotFound, ProjectData.Users[4]);
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

    private async Task AssertNotFoundResponse(Error error, UserAggregate user, ProjectAggregate? project = null)
    {
        var outcome = await ArrangeAndAct(user, project);

        // Assert
        await outcome.ValidateError(error);
    }

    private async Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user, ProjectAggregate? project = null)
    {
        // Arrange
        project ??= ProjectData.Projects[^2];
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        return await Client.Delete($"api/projects/{request.Id}/leave");
    }
}