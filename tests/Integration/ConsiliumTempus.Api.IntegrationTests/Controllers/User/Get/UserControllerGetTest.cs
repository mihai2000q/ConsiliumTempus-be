using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Get;

[Collection(nameof(UserControllerCollection))]
public class UserControllerGetTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task GetUser_WhenIsSuccessful_ShouldReturnUser()
    {
        // Arrange
        var user = UserData.Users.First();
        
        // Act
        var outcome = await Client.Get($"api/users/{user.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<GetUserResponse>();
        Utils.User.AssertGetResponse(response!, user);
    }
    
    [Fact]
    public async Task GetUser_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        
        // Act
        var outcome = await Client.Get($"api/users/{id}");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}