using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.RemoveStatus;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerRemoveStatusValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task RemoveStatusFromProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var status = project.Statuses[0];
        var request = ProjectRequestFactory.CreateRemoveStatusFromProjectRequest(
            project.Id.Value,
            status.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Delete("api/projects/" +
                                          $"{request.Id}/Remove-Status/{request.StatusId}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RemoveStatusFromProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateRemoveStatusFromProjectRequest(
            id: Guid.Empty,
            statusId: Guid.Empty);

        // Act
        var outcome = await Client.Delete("api/projects/" +
                                          $"{request.Id}/Remove-Status/{request.StatusId}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}