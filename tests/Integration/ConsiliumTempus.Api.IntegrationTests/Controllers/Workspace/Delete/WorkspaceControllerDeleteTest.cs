using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Delete;

public class WorkspaceControllerDeleteTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WorkspaceDelete_WhenItSucceeds_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        const string id = "30000000-0000-0000-0000-000000000000";

        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteWorkspaceResponse>();
        response!.Message.Should().Be("Workspace has been deleted successfully!");
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(2);
        dbContext.Workspaces.AsEnumerable()
            .SingleOrDefault(w => w.Id.Value.ToString() == id)
            .Should().BeNull();
    }

    [Fact]
    public async Task WhenWorkspaceDeleteFails_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "50000000-0000-0000-0000-000000000000";

        // Act
        var outcome = await Client.DeleteAsync($"api/workspaces/{id}");

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Workspaces.Should().HaveCount(3);
        dbContext.Workspaces.AsEnumerable()
            .SingleOrDefault(w => w.Id.Value.ToString() == id)
            .Should().BeNull();
    }
}