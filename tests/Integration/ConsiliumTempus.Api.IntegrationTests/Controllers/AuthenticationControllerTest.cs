using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers;

public class AuthenticationControllerTest : BaseIntegrationTest
{
    public AuthenticationControllerTest(ConsiliumTempusWebApplicationFactory factory) : base(factory)
    {
    }
    
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
        response?.Token.Should().NotBeNullOrWhiteSpace();
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