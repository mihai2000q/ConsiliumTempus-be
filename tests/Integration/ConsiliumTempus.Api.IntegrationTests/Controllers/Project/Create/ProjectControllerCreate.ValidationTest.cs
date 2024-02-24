using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project.Create;

public class ProjectControllerCreateValidationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Project")
{
    [Fact]
    public async Task WhenProjectCreateCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest(
            new Guid("10000000-0000-0000-0000-000000000000"));

        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.PostAsJsonAsync("api/projects", request);
        
        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WhenProjectCreateCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectRequestFactory.CreateCreateProjectRequest(workspaceId: Guid.Empty, name: string.Empty);

        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.PostAsJsonAsync("api/projects", request);
        
        // Assert
        await outcome.ValidateValidationErrors();
    }
}