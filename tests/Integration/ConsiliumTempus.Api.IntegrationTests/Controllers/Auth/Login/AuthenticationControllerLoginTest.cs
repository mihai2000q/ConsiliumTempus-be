using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Login;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerLoginTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Auth", false)
{
    [Fact]
    public async Task Login_WhenIsSuccessful_ShouldCreateRefreshTokenAndReturnTokens()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(
            email: "MichaelJ@Gmail.com",
            password: "MichaelJordan2");

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<LoginResponse>();
        Utils.Auth.AssertToken(response?.Token, JwtSettings, request.Email.ToLower());
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<RefreshToken>().Should().HaveCount(1);
        var refreshToken = await dbContext.Set<RefreshToken>().SingleAsync();
        var user = await dbContext.Users.SingleAsync(u => u.Credentials.Email == request.Email.ToLower());
        Utils.RefreshToken.AssertCreation(
            refreshToken, 
            response?.RefreshToken, 
            response?.Token, 
            user);
    }

    [Fact]
    public async Task Login_WhenUserIsNotFound_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(email: "FirstLast@Example.com");
        
        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidCredentials);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users
            .SingleOrDefault(u => u.Credentials.Email == request.Email.ToLower())
            .Should().BeNull();
    }

    [Fact]
    public async Task Login_WhenPasswordIsWrong_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(
            email: "MichaelJ@Gmail.com",
            password: "Password123");

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidCredentials);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users
            .SingleOrDefault(u => u.Credentials.Email == request.Email.ToLower())
            .Should().NotBeNull();
    }
}