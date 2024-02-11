using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User;

public class UserControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenUpdateUserIsSuccessful_ThenReturnNewUser()
    {
        // Arrange
        const string email = "michaelj@gmail.com";
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/users/{id}");

        // Assert
        await Utils.User.AssertDtoFromResponse(
            outcome,
            "Michael",
            "Jordan",
            email);
    }
    
    [Fact]
    public async Task WhenUpdateUserIsNotOwner_ThenReturnForbiddenResponse()
    {
        // Arrange
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.DeleteAsync($"api/users/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task WhenUpdateUserIsNotFound_ThenReturnNotFoundError()
    {
        // Arrange
        const string id = "90000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/users/{id}");

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, Errors.User.NotFound.Description);
    }
}