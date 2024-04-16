using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.UpdateCurrent;

[Collection(nameof(UserControllerCollection))]
public class UserControllerUpdateValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task UpdateCurrentUser_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateCurrentUserRequest();

        // Act
        var outcome = await Client.Put("api/users/current", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCurrentUser_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateCurrentUserRequest(
            firstName: string.Empty, 
            role: new string('a', 1000));

        // Act
        var outcome = await Client.Put("api/users/current", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}