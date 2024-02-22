using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

public class UserControllerUpdateAuthorizationTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task UpdateUser_WhenUserIsOwner_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new UpdateUserRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "New First Name", 
            "New Lastname",
            "Software Developer",
            null);
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateUser_WhenUserIsNotOwner_ShouldReturnForbiddenResponse()
    {
        // Arrange
        var request = new UpdateUserRequest(
            new Guid("10000000-0000-0000-0000-000000000000"),
            "New First Name", 
            "New Lastname",
            null,
            null);
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}