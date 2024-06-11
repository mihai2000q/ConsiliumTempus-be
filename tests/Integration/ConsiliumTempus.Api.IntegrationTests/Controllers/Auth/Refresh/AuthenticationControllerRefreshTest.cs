using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Authentication;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Refresh;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerRefreshTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new AuthData(), true)
{
    [Fact]
    public async Task Refresh_WhenItIsSuccessful_ShouldReturnNewToken()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens.First();
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings, refreshToken.JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            Utils.Token.SecurityTokenToStringToken(token),
            refreshToken.Id.Value);

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<RefreshResponse>();

        Utils.Auth.AssertToken(
            response!.Token,
            JwtSettings,
            refreshToken.User);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedRefreshToken = await dbContext.Set<RefreshToken>()
            .SingleAsync(rt => rt.Id == RefreshTokenId.Create(request.RefreshToken));

        Utils.RefreshToken.AssertRefresh(refreshToken, updatedRefreshToken, response.Token);
    }
    
    [Fact]
    public async Task Refresh_WhenItHasAlreadyBeenRefreshed_ShouldReturnTokenWithSameJwtId()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[1];
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings, refreshToken.History[0].JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            Utils.Token.SecurityTokenToStringToken(token),
            refreshToken.Id.Value);

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<RefreshResponse>();

        Utils.Auth.AssertToken(
            response!.Token,
            JwtSettings,
            refreshToken.User);

        Utils.RefreshToken.AssertAlreadyRefreshed(refreshToken, response.Token);
    }

    [Fact]
    public async Task Refresh_WhenAccessTokenIsInvalidDueToJwtSettings_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[0];
        var token = Utils.Token.GenerateInvalidToken();
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            Utils.Token.SecurityTokenToStringToken(token),
            refreshToken.Id.Value);

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }

    [Fact]
    public async Task Refresh_WhenAccessTokenIsInvalidDueToClaims_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[0];
        var token = Utils.Token.GenerateInvalidToken(JwtSettings);
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            Utils.Token.SecurityTokenToStringToken(token),
            refreshToken.Id.Value);

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }

    [Fact]
    public async Task Refresh_WhenRefreshTokenIsInvalidated_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[2];
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings, refreshToken.JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            Utils.Token.SecurityTokenToStringToken(token),
            refreshToken.Id.Value);

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }

    [Fact]
    public async Task Refresh_WhenRefreshTokenIsExpired_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[3];
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings, refreshToken.JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            Utils.Token.SecurityTokenToStringToken(token),
            refreshToken.Id.Value);

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }

    [Fact]
    public async Task Refresh_WhenRefreshTokenJwtIdIsWrong_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[4];
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings);
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            Utils.Token.SecurityTokenToStringToken(token),
            refreshToken.Id.Value);

        // Act
        var outcome = await Client.Put("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
}