using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace;

public class WorkspaceControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WhenWorkspaceDeleteWithAdminRole_ShouldReturnWorkspace()
    {
        await AssertSuccessfulRequest("michaelj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteWithMemberRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("stephenc@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteWithViewRole_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("lebronj@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteWithoutMembership_ShouldReturnForbiddenResponse()
    {
        await AssertForbiddenResponse("leom@gmail.com");
    }

    [Fact]
    public async Task WhenWorkspaceDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "50000000-0000-0000-0000-000000000000";

        // Act
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        DbContext.Workspaces.Should().HaveCount(3);
        DbContext.Workspaces.AsEnumerable()
            .SingleOrDefault(w => w.Id.Value.ToString() == id)
            .Should().BeNull();
        
        await outcome.ValidateError(HttpStatusCode.NotFound, Errors.Workspace.NotFound.Description);
    }

    private async Task AssertSuccessfulRequest(string email)
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";

        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        DbContext.Workspaces.Should().HaveCount(2);
        DbContext.Workspaces.AsEnumerable()
            .SingleOrDefault(w => w.Id.Value.ToString() == id)
            .Should().BeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteWorkspaceResponse>();
        response!.Message.Should().Be("Workspace has been deleted successfully!");
    }

    private async Task AssertForbiddenResponse(string email)
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";

        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        DbContext.Workspaces.Should().HaveCount(3);
        DbContext.Workspaces.AsEnumerable()
            .SingleOrDefault(w => w.Id.Value.ToString() == id)
            .Should().NotBeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}