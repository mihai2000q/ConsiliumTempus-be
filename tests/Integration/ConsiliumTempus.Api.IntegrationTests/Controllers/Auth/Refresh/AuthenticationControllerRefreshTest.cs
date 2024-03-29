using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestFactory.Request;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Refresh;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerRefreshTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new AuthData(), false)
{
    [Fact]
    public async Task Refresh_WhenItIsSuccessful_ShouldReturnNewTokens()
    {
        // Arrange
        var refreshTokenRequest = AuthData.RefreshTokens.First();
        var token = Utils.Token.GenerateValidToken(refreshTokenRequest.User, JwtSettings, refreshTokenRequest.JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshTokenRequest.Value,
            Utils.Token.SecurityTokenToStringToken(token));

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<RefreshResponse>();
        
        Utils.Auth.AssertToken(
            response?.Token, 
            JwtSettings, 
            refreshTokenRequest.User);

        var dbContext = await DbContextFactory.CreateDbContextAsync();
        var refreshToken = await dbContext.Set<RefreshToken>()
            .SingleAsync(rt => rt.Id.ToString() == request.RefreshToken);

        refreshToken.UsedTimes.Should().Be(1);
        refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }
    
    [Fact]
    public async Task Refresh_WhenAccessTokenIsInvalidDueToJwtSettings_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[0].Value;
        var token = Utils.Token.GenerateInvalidToken();
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken, 
            Utils.Token.SecurityTokenToStringToken(token));

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task Refresh_WhenAccessTokenIsInvalidDueToClaims_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[0].Value;
        var token = Utils.Token.GenerateInvalidToken(JwtSettings);
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken, 
            Utils.Token.SecurityTokenToStringToken(token));

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task Refresh_WhenRefreshTokenIsInvalidated_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[1];
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings, refreshToken.JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken.Value,
            Utils.Token.SecurityTokenToStringToken(token));

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task Refresh_WhenRefreshTokenIsExpired_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[2];
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings, refreshToken.JwtId.ToString());
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken.Value,
            Utils.Token.SecurityTokenToStringToken(token));

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task Refresh_WhenRefreshTokenJwtIdIsWrong_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var refreshToken = AuthData.RefreshTokens[3];
        var token = Utils.Token.GenerateValidToken(refreshToken.User, JwtSettings);
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken.Value,
            Utils.Token.SecurityTokenToStringToken(token));

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
}