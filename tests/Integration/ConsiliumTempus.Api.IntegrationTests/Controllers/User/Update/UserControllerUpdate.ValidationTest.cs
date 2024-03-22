using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

[Collection(nameof(UserControllerCollection))]
public class UserControllerUpdateValidationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task UpdateUser_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest();
        
        // Act
        Client.UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.Put("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateUser_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest(
            firstName: string.Empty, 
            role: new string('a', 1000));
        
        // Act
        Client.UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.Put("api/users", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}