using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Auth", false)
{
    [Fact]
    public async Task Register_WhenIsSuccessful_ShouldAddNewUserAndReturnToken()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest(email: "FirstLast@Example.com");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<RegisterResponse>();
        Utils.Auth.AssertToken(
            response?.Token,
            JwtSettings,
            request.Email.ToLower(),
            request.FirstName.CapitalizeWord(),
            request.LastName.CapitalizeWord());
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(2);
        var createdUser = dbContext.Users
            .Include(u => u.Memberships)
            .ThenInclude(m => m.Workspace)
            .Single(u => u.Credentials.Email == request.Email.ToLower());
        Utils.User.AssertRegistration(createdUser, request);
    }

    [Fact]
    public async Task Register_WhenItFails_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest(email: "MichaelJ@Gmail.com");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);

        // Assert
        await outcome.ValidateError(Errors.User.DuplicateEmail);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(1);
        dbContext.Users.SingleOrDefault(u => u.Credentials.Email == request.Email.ToLower())
            .Should().NotBeNull();
    }

    [Fact]
    public async Task Login_WhenIsSuccessful_ShouldReturnToken()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(
            email: "MichaelJ@Gmail.com",
            password: "MichaelJordan2");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<LoginResponse>();
        Utils.Auth.AssertToken(response?.Token, JwtSettings, request.Email.ToLower());
    }

    [Fact]
    public async Task Login_WhenUserIsNotFound_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(email: "FirstLast@Example.com");
        
        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);

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
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);

        // Assert
        await outcome.ValidateError(Errors.Authentication.InvalidCredentials);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users
            .SingleOrDefault(u => u.Credentials.Email == request.Email.ToLower())
            .Should().NotBeNull();
    }
}