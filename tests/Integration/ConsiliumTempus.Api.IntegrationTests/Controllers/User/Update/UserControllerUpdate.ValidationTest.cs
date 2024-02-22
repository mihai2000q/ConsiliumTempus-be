using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

public class UserControllerUpdateValidationTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task UpdateUser_WhenCommandIsValid_ShouldReturnSuccessResponse()
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
    public async Task UpdateUser_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = new UpdateUserRequest(
            Guid.Empty, 
            "New First Name", 
            "New Lastname",
            new string('a', 1000),
            null);
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}