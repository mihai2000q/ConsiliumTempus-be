using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Authentication;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Login;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerLoginValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new AuthData(), true)
{
    [Fact]
    public async Task Login_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest();

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(email: "no email");

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}