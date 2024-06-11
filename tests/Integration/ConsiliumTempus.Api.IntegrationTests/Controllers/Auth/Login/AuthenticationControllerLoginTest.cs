using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Authentication;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Login;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerLoginTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new AuthData(), true)
{
    [Fact]
    public async Task Login_WhenIsSuccessful_ShouldCreateRefreshTokenAndReturnTokens()
    {
        // Arrange
        var user = AuthData.Users.First();
        var request = AuthenticationRequestFactory.CreateLoginRequest(
            email: user.Credentials.Email);

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<LoginResponse>();
        Utils.Auth.AssertToken(response!.Token, JwtSettings, user);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<RefreshToken>().Should().HaveCount(AuthData.RefreshTokens.Length + 1);
        var refreshToken = await dbContext.Set<RefreshToken>()
            .Include(rt => rt.User)
            .Where(rt => rt.User == user)
            .OrderBy(rt => rt.CreatedDateTime)
            .LastAsync();
        Utils.RefreshToken.AssertCreation(
            refreshToken,
            response!.RefreshToken,
            response.Token,
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

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users
            .SingleOrDefault(u => u.Credentials.Email == request.Email.ToLower())
            .Should().BeNull();

        dbContext.Set<RefreshToken>().Should().HaveCount(AuthData.RefreshTokens.Length);
    }

    [Fact]
    public async Task Login_WhenPasswordIsWrong_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(
            email: AuthData.Users.First().Credentials.Email,
            password: "Password123");

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidCredentials);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users
            .SingleOrDefault(u => u.Credentials.Email == request.Email.ToLower())
            .Should().NotBeNull();

        dbContext.Set<RefreshToken>().Should().HaveCount(AuthData.RefreshTokens.Length);
    }
}