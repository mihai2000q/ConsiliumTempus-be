using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.GetCurrent;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.GetCurrent;

[Collection(nameof(UserControllerCollection))]
public class UserControllerGetCurrentTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task WhenGetCurrentUserIsSuccessful_ShouldReturnCurrentUser()
    {
        // Arrange
        var user = UserData.Users.First();
        
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get("api/users/current");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetCurrentUserResponse>();
        Utils.User.AssertGetCurrentResponse(response!, user);
    }
    
    [Fact]
    public async Task WhenGetCurrentUserFails_ShouldReturnNotFoundError()
    {
        // Arrange
        
        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Get("api/users/current");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}