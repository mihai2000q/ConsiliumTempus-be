using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.UpdateOwner;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerUpdateOwnerValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task UpdateOwnerProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var collaborator = ProjectData.Users[3];
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateUpdateOwnerProjectRequest(
            project.Id.Value,
            collaborator.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Put("api/projects/Owner", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateOwnerProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateUpdateOwnerProjectRequest(
            id: Guid.Empty);

        // Act
        var outcome = await Client.Put("api/projects/Owner", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}