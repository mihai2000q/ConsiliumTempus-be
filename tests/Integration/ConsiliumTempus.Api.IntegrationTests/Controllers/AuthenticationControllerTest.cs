using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers;

public class AuthenticationControllerTest(ConsiliumTempusWebApplicationFactory factory)
    : BaseIntegrationTest(factory, "AuthData", false)
{
    [Fact]
    public async Task WhenRegisterIsSuccessful_ShouldReturnToken()
    {
        // Arrange
        var request = new RegisterRequest(
            "First",
            "Last",
            "FirstLast@Example.com",
            "Password123");
        
        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);
    
        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<RegisterResponse>();
        Utils.Auth.AssertToken(response?.Token, JwtSettings, request.Email, request.FirstName, request.LastName);
    }
    
    [Fact]
    public async Task WhenRegisterFails_ShouldReturnDuplicateEmailError()
    {
        // Arrange
        var request = new RegisterRequest(
            "Michael",
            "Jordan",
            "MichaelJ@Gmail.com",
            "MichaelJordan2");
        
        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);
    
        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var error = await outcome.Content.ReadFromJsonAsync<ProblemDetails>();
        error?.Title.Should().Be("Email is already in use");
        error?.Status.Should().Be(StatusCodes.Status409Conflict);
        var errorCodes = error?.Extensions["errorCodes"] as JsonElement?;
        errorCodes?.ValueKind.Should().Be(JsonValueKind.Array);
    }
    
    [Fact]
    public async Task WhenLoginIsSuccessful_ShouldReturnToken()
    {
        // Arrange
        var request = new LoginRequest(
            "MichaelJ@Gmail.com",
            "MichaelJordan2");
        
        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);
    
        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await outcome.Content.ReadFromJsonAsync<LoginResponse>();
        Utils.Auth.AssertToken(response?.Token, JwtSettings, request.Email);
    }
    
    [Fact]
    public async Task WhenLoginFails_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var request = new LoginRequest(
            "FirstLast@Example.com",
            "Password123");
        
        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);
    
        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var error = await outcome.Content.ReadFromJsonAsync<ProblemDetails>();
        error?.Title.Should().Be("Invalid Credentials");
        error?.Status.Should().Be(StatusCodes.Status401Unauthorized);
        var errorCodes = error?.Extensions["errorCodes"] as JsonElement?;
        errorCodes?.ValueKind.Should().Be(JsonValueKind.Array);
    }
}