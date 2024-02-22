using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectSprint.Create;

public class ProjectSprintControllerCreateValidationTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "ProjectSprint")
{
    
    [Fact]
    public async Task WhenProjectSprintCreateCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new CreateProjectSprintRequest(
            new Guid("20000000-0000-0000-0000-000000000000"),
            "Sprint 2 - Qualify on Semi-Finals",
            null,
            null);
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/projects/sprints", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WhenProjectSprintCreateCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = new CreateProjectSprintRequest(
            Guid.Empty,
            string.Empty,
            null,
            null);
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/projects/sprints", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}