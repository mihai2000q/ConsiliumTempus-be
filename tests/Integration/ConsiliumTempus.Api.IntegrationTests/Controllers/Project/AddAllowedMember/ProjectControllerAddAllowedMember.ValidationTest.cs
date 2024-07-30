using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.AddAllowedMember;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerAddAllowedMemberValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task AddAllowedMemberToProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects[^1];
        var collaborator = ProjectData.Users[4];
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest(
            project.Id.Value,
            collaborator.Id.Value);

        // Act
        Client.UseCustomToken(project.Owner);
        var outcome = await Client.Post("api/projects/Add-Allowed-Member", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddAllowedMemberToProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateAddAllowedMemberToProjectRequest(
            id: Guid.Empty,
            collaboratorId: Guid.Empty);

        // Act
        var outcome = await Client.Post("api/projects/Add-Allowed-Member", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}