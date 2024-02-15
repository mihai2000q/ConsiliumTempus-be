using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Project;

public class ProjectControllerCreateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Project")
{
    [Fact]
    public async Task WhenProjectCreateWithAdminRole_ShouldReturnSuccessResponse()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenProjectCreateWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenProjectCreateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenProjectCreateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    [Fact]
    public async Task WhenProjectCreateFails_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange - parameters
        var request = new CreateProjectRequest(
            new Guid("90000000-0000-0000-0000-000000000000"), 
            "Project Name",
            "This is the project description",
            true);
        
        // Act
        var outcome = await Client.PostAsJsonAsync("api/projects", request);

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, Errors.Workspace.NotFound.Description);
    }

    private async Task AssertSuccessfulRequest(string email)
    {
        var outcome = await ArrangeAndAct(email);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateProjectResponse>();
        response!.Message.Should().Be("Project created successfully!");
    }
    
    private async Task AssertForbiddenResponse(string email)
    {
        var outcome = await ArrangeAndAct(email);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private Task<HttpResponseMessage> ArrangeAndAct(string email)
    {
        // Arrange - parameters
        var request = new CreateProjectRequest(
            new Guid("10000000-0000-0000-0000-000000000000"), 
            "Project Name",
            "This is the project description",
            true);
        
        // Act
        UseCustomToken(email);
        return Client.PostAsJsonAsync("api/projects", request);
    }
}