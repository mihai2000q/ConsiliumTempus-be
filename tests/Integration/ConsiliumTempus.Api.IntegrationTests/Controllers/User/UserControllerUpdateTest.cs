using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User;

public class UserControllerUpdateTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenUpdateUserIsSuccessful_ThenReturnNewUser()
    {
        // Arrange
        const string email = "michaelj@gmail.com";
        var request = GetRequest();
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        await Utils.User.AssertDtoFromResponse(outcome, request.FirstName!, request.LastName!, email);
    }
    
    [Fact]
    public async Task WhenUpdateUserIsNotOwner_ThenReturnForbiddenResponse()
    {
        // Arrange
        var request = GetRequest();
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task WhenUpdateUserIsNotFound_ThenReturnNotFoundError()
    {
        // Arrange
        var request = GetRequest("90000000-0000-0000-0000-000000000000");
        
        // Act
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        await outcome.ValidateError(HttpStatusCode.NotFound, Errors.User.NotFound.Description);
    }

    private static UpdateUserRequest GetRequest(string id = "10000000-0000-0000-0000-000000000000")
    {
        return new UpdateUserRequest(
            new Guid(id),
            "New First Name", 
            "New Lastname");
    }
}