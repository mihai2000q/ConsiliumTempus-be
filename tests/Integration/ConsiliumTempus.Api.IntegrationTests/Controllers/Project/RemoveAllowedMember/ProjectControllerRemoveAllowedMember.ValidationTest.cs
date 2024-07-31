using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.RemoveAllowedMember;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerRemoveAllowedMemberValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task RemoveAllowedMemberFromProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects[^1];
        var collaborator = ProjectData.Users[4];
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest(
            project.Id.Value,
            collaborator.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Delete($"api/projects/" +
                                          $"{request.Id}/Remove-Allowed-Member/{request.AllowedMemberId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RemoveAllowedMemberFromProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveAllowedMemberFromProjectRequest(
            id: Guid.Empty,
            allowedMemberId: Guid.Empty);

        // Act
        var outcome = await Client.Delete($"api/projects/" +
                                          $"{request.Id}/Remove-Allowed-Member/{request.AllowedMemberId}");
        // Assert
        await outcome.ValidateValidationErrors();
    }
}