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
    public async Task WhenProjectSprintGetCollectionWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(ProjectSprintData.Users[0]);
    }
    
    [Fact]
    public async Task WhenProjectSprintGetCollectionWithMemberRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(ProjectSprintData.Users[3]);
    }

    [Fact]
    public async Task WhenProjectGetCollectionWithViewRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest(ProjectSprintData.Users[4]);
    }

    [Fact]
    public async Task WhenProjectSprintGetCollectionWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse(ProjectSprintData.Users[1]);
    }

    private async Task AssertSuccessfulRequest(UserAggregate user)
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