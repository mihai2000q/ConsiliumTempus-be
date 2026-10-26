using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Get;

[Collection(nameof(UserControllerCollection))]
public class UserControllerGetValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task GetUser_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = UserRequestFactory.CreateGetUserRequest(UserData.Users.First().Id.Value);

        // Act
        var outcome = await Client.Get($"api/users/{request.Id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUser_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = UserRequestFactory.CreateGetUserRequest(Guid.Empty);

        // Act
        var outcome = await Client.Get($"api/users/{request.Id}");

        // Assert
        await outcome.ValidateValidationErrors();
    }
}
