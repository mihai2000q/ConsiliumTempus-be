using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Workspace.Update;

[Collection(nameof(WorkspaceControllerCollection))]
public class WorkspaceControllerUpdateValidationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper, "Workspace")
{
    [Fact]
    public async Task WorkspaceUpdate_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest(
            id: new Guid("10000000-0000-0000-0000-000000000000"));
        
        // Act
        Client.UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.Put("api/workspaces", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task WorkspaceUpdate_WhenCommandIsInvalid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = WorkspaceRequestFactory.CreateUpdateWorkspaceRequest(
            id: Guid.Empty, 
            name: string.Empty);
        
        // Act
        Client.UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.Put("api/workspaces", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}