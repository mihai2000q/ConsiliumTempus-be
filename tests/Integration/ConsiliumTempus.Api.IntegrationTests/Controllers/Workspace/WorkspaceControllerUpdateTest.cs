using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerUpdateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceUpdateWithAdminRole_ShouldReturnNewWorkspace()
    {
        await AssertSuccessfulRequest(GetUpdateRequest(), "michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithMemberRole_ShouldReturnNewWorkspace()
    {
        await AssertSuccessfulRequest(GetUpdateRequest(), "stephenc@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest(GetUpdateRequest(), "lebronj@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenRequest(GetUpdateRequest(), "leom@gmail.com");
    }
    
    [Fact]
    public async Task WhenWorkspaceUpdateFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = GetUpdateRequest("90000000-0000-0000-0000-000000000000");
        
        // Act
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, Errors.Workspace.NotFound.Description);
    }

    private async Task AssertSuccessfulRequest(UpdateWorkspaceRequest request, string email)
    {
        // Arrange - parameters
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        await Utils.Workspace.AssertDtoFromResponse(
            outcome, 
            request.Name!, 
            request.Description!);
    }

    private async Task AssertForbiddenRequest(UpdateWorkspaceRequest request, string email)
    {
        // Arrange - parameters
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PutAsJsonAsync("api/workspaces", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private static UpdateWorkspaceRequest GetUpdateRequest(string id = "10000000-0000-0000-0000-000000000000")
    {
        return new UpdateWorkspaceRequest(
            new Guid(id),
            "Workspace New Name",
            "This is a new description");
    }
}