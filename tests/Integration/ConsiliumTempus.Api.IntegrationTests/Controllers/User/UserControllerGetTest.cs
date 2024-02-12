using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User;

public class UserControllerGetTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenUpdateUserIsSuccessful_ThenReturnNewUser()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.GetAsync($"api/users/{id}");

        // Assert
        await Utils.User.AssertDtoFromResponse(
            outcome, 
            "Michael", 
            "Jordan", 
            "michaelj@gmail.com");
    }
    
    [Fact]
    public async Task WhenUpdateUserIsNotFound_ThenReturnNotFoundError()
    {
        // Arrange
        const string id = "90000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.GetAsync($"api/users/{id}");

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, Errors.User.NotFound.Description);
    }
}