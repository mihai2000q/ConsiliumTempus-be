using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Get;

public class UserControllerGetTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenGetUserIsSuccessful_ThenReturnNewUser()
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
            "michaelj@gmail.com",
            id,
            "Pro Basketball Player",
            DateOnly.Parse("2000-12-23"));
    }
    
    [Fact]
    public async Task WhenGetUserIsNotFound_ThenReturnNotFoundError()
    {
        // Arrange
        const string id = "90000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.GetAsync($"api/users/{id}");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}