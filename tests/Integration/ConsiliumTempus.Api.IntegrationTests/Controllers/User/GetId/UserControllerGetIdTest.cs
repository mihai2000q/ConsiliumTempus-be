using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.GetId;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.GetId;

[Collection(nameof(UserControllerCollection))]
public class UserControllerGetIdTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task WhenGetUserIdIsSuccessful_ThenReturnTheUserId()
    {
        // Arrange
        var user = UserData.Users.First();
        
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Get($"api/users/id");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await outcome.Content.ReadFromJsonAsync<GetUserIdResponse>();
        content!.Id.Should().Be(user.Id.ToString());
    }
}