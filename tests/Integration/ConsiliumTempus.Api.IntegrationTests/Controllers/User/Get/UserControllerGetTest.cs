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
    public async Task WhenGetUserIsSuccessful_ThenReturnUser()
    {
        // Arrange
        var user = UserData.Users.First();
        
        // Act
        var outcome = await Client.Get($"api/users/{user.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        await Utils.User.AssertDtoFromResponse(outcome, user);
    }
    
    [Fact]
    public async Task WhenGetUserIsNotFound_ThenReturnNotFoundError()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        
        // Act
        var outcome = await Client.Get($"api/users/{id}");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}