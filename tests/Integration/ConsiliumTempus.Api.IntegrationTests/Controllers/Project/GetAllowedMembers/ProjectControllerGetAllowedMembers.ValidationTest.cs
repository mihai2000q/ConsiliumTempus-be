using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetAllowedMembers;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetAllowedMembersValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetAllowedMembersFromProject_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateGetAllowedMembersFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get($"api/projects/{request.Id}/allowed-members");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllowedMembersProject_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetAllowedMembersFromProjectRequest(Guid.Empty);

        // Act
        var outcome = await Client.Get($"api/projects/{request.Id}/allowed-members");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}