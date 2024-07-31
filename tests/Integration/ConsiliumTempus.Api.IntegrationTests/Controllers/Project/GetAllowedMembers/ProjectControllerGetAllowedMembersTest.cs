using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.GetAllowedMembers;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetAllowedMembers;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetAllowedMembersTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetAllowedMembersFromProject_WhenSucceeds_ShouldReturnProject()
    {
        // Arrange
        var user = ProjectData.Users.First();
        var project = ProjectData.Projects.First();
        var request = ProjectRequestFactory.CreateGetAllowedMembersFromProjectRequest(project.Id.Value);

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/projects/{request.Id}/allowed-members");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetAllowedMembersFromProjectResponse>();
        Utils.Project.AssertGetAllowedMembersResponse(response!, project.AllowedMembers);
    }

    [Fact]
    public async Task GetAllowedMembersFromProject_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetAllowedMembersFromProjectRequest();

        // Act
        var outcome = await Client.Get($"api/projects/{request.Id}/allowed-members");

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);
    }
}