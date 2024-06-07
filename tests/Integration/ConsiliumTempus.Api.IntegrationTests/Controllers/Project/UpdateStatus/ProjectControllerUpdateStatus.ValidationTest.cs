using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateStatus;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateStatusValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateStatusFromProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var status = project.Statuses[0];
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest(
            project.Id.Value,
            status.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Put("api/projects/Update-Status", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateStatusFromProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateStatusFromProjectRequest(
            id: Guid.Empty,
            statusId: Guid.Empty);

        // Act
        var outcome = await Client.Put("api/projects/Update-Status", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}