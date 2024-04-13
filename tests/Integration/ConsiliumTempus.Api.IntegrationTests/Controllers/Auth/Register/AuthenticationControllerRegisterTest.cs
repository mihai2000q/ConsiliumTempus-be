using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Authentication;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Register;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerRegisterTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new AuthData(), true)
{
    [Fact]
    public async Task Register_WhenIsSuccessful_ShouldAddNewUserCreateRefreshTokenAndReturnTokens()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest(email: "FirstLast@Example.com");

        // Act
        var outcome = await Client.Post("/api/auth/Register", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        // assert registered user in db
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(AuthData.Users.Length + 1);
        var user = dbContext.Users
            .Include(u => u.Memberships)
            .ThenInclude(m => m.Workspace)
            .ThenInclude(w => w.Owner)
            .Include(u => u.Memberships)
            .Single(u => u.Credentials.Email == request.Email.ToLower());
        Utils.User.AssertRegistration(user, request);
        
        // assert returned access token
        var response = await outcome.Content.ReadFromJsonAsync<RegisterResponse>();
        Utils.Auth.AssertToken(
            response?.Token,
            JwtSettings,
            user);

        // assert created refresh token in db
        dbContext.Set<RefreshToken>().Should().HaveCount(AuthData.RefreshTokens.Length + 1);
        var refreshToken = await dbContext.Set<RefreshToken>()
            .Where(rt => rt.User == user)
            .OrderBy(rt => rt.CreatedDateTime)
            .LastAsync();
        Utils.RefreshToken.AssertCreation(
            refreshToken, 
            response?.RefreshToken, 
            response?.Token, 
            user);
    }

    [Fact]
    public async Task Register_WhenItFails_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest(email: AuthData.Users.First().Credentials.Email);

        // Act
        var outcome = await Client.Post("/api/auth/Register", request);

        // Assert
        // assert returned error
        await outcome.ValidateError(Errors.User.DuplicateEmail);
        
        // assert users in db
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(AuthData.Users.Length);
        dbContext.Users.SingleOrDefault(u => u.Credentials.Email == request.Email.ToLower())
            .Should().NotBeNull();
        
        // assert refreshTokens in db
        dbContext.Set<RefreshToken>().Should().HaveCount(AuthData.RefreshTokens.Length);
    }
}