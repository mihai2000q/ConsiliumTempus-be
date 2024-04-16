using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Authentication;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Refresh;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerRefreshValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new AuthData(), true)
{
    [Fact]
    public async Task Refresh_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens.First();
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings, refreshToken.JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken.Value,
            Utils.Token.SecurityTokenToStringToken(token));

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Refresh_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest(refreshToken: "", token: "");

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}