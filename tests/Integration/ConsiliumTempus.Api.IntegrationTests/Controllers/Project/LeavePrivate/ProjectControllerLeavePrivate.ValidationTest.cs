using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.LeavePrivate;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerLeavePrivateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task LeavePrivateProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects[^2];
        var user = project.AllowedMembers.First(u => u != project.Owner);
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete($"api/projects/{request.Id}/Leave");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task LeavePrivateProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateLeavePrivateProjectRequest(
            id: Guid.Empty);

        // Act
        var outcome = await Client.Delete($"api/projects/{request.Id}/Leave");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}