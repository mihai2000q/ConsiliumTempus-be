using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Create;

[Collection(nameof(ProjectSprintControllerCollection))]
public class ProjectSprintControllerCreateValidationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "ProjectSprint")
{
    
    [Fact]
    public async Task WhenProjectSprintCreateCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(
            new Guid("10000000-0000-0000-0000-000000000000"));
        
        // Act
        Client.UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WhenProjectSprintCreateCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectSprintRequestFactory.CreateCreateProjectSprintRequest(
            projectId: Guid.Empty, 
            name: string.Empty);  
        
        // Act
        Client.UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.Post("api/projects/sprints", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}