using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Common.IntegrationTests.Project.Entities;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.GetCollection;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerGetCollectionAuthorizationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectSprintData())
{
    [Fact]
    public async Task GetCollectionProjectSprint_WhenWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[0]);
    }

    [Fact]
    public async Task GetCollectionProjectSprint_WhenWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[3]);
    }

    [Fact]
    public async Task GetCollectionProjectSprint_WhenWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulResponse(ProjectSprintData.Users[4]);
    }

    [Fact]
    public async Task GetCollectionProjectSprint_WhenWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[1]);
    }

    private async Task AssertSuccessfulResponse(UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task AssertForbiddenResponse(UserAggregate user)
    {
        var outcome = await ArrangeAndAct(user);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private Task<HttpResponseMessage> ArrangeAndAct(UserAggregate user)
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateGetCollectionProjectSprintRequest(
            ProjectSprintData.Projects.First().Id.Value);

        // Act
        Client.UseCustomToken(user);
        return Client.Get($"api/projects/sprints?projectId={request.ProjectId}");
    }
}