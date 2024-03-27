using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestConstants;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Refresh;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerRefreshTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Auth/Refresh", false)
{
    [Fact]
    public async Task Refresh_WhenItIsSuccessful_ShouldReturnNewTokens()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest();

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<RefreshResponse>();
        
        Utils.Auth.AssertToken(
            response?.Token, 
            JwtSettings, 
            "michaelj@gmail.com",
            "Michael",
            "Jordan");

        var dbContext = await DbContextFactory.CreateDbContextAsync();
        var refreshToken = await dbContext.Set<RefreshToken>()
            .SingleAsync(rt => rt.Id.ToString() == request.RefreshToken);

        refreshToken.UsedTimes.Should().Be(1);
        refreshToken.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }
    
    [Fact]
    public async Task Refresh_WhenAccessTokenIsInvalid_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest(token: Constants.InvalidToken);

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task Refresh_WhenRefreshTokenIsInvalidated_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken: "10000000-0000-0000-0000-000000000097");

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task Refresh_WhenRefreshTokenIsExpired_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken: "10000000-0000-0000-0000-000000000098");

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
    
    [Fact]
    public async Task Refresh_WhenRefreshTokenJwtIdIsWrong_ShouldReturnInvalidTokensError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest(
            refreshToken: "10000000-0000-0000-0000-000000000099");

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidTokens);
    }
}