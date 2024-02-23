using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
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
        var request = UserRequestFactory.CreateUpdateUserRequest(
            id: new Guid("10000000-0000-0000-0000-000000000000"));
        
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
        var request = UserRequestFactory.CreateUpdateUserRequest(
            id: Guid.Empty, 
            firstName: string.Empty, 
            role: new string('a', 1000));
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}