using System.Net.Http.Json;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.GetCollection;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerGetCollectionTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new WorkspaceData())
{
    [Fact]
    public async Task WhenWorkspaceGetCollectionForUserIsSuccessful_ShouldReturnAllTheWorkspacesForUser()
    {
        // Arrange
        var user = WorkspaceData.Users.First();
        var workspaces = user.Memberships.Select(m => m.Workspace)
            .OrderBy(w => w.Name.Value)
            .ToList();
        
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get("api/Workspaces");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await outcome.Content.ReadFromJsonAsync<List<WorkspaceDto>>();
        content.Should().HaveCount(workspaces.Count);
        foreach (var x in content!.OrderBy(c => c.Name).Zip(workspaces))
        {
            Utils.Workspace.AssertDto(x.First, x.Second);
        }
    }
    
    [Fact]
    public async Task WhenWorkspaceGetCollectionForUserFails_ShouldReturnUserNotFoundError()
    {
        // Arrange
        
        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Get("api/Workspaces");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}