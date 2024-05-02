using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Project;
using ConsiliumTempus.Domain.Common.Validation;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.GetCollection;

[Collection(nameof(ProjectControllerCollection))]
public class ProjectControllerGetCollectionValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectData())
{
    [Fact]
    public async Task GetProjectCollection_WhenQueryIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange

        // Act
        Client.UseCustomToken(ProjectData.Users.First());
        var outcome = await Client.Get("api/projects");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProjectCollection_WhenQueryIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateGetCollectionProjectRequest(
            name: new string('*', PropertiesValidation.Project.NameMaximumLength + 1),
            pageSize: -1);

        // Act
        var outcome = await Client.Get($"api/projects?name={request.Name}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}