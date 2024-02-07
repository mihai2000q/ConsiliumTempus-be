using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Application.Common.Extensions;
using FluentAssertions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth;

public class AuthenticationControllerTest(ConsiliumTempusWebApplicationFactory factory)
    : BaseIntegrationTest(factory, "Auth", false)
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
        Utils.Auth.AssertToken(
            response?.Token,
            JwtSettings, 
            request.Email.ToLower(), 
            request.FirstName.CapitalizeWord(), 
            request.LastName.CapitalizeWord());
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
        await outcome.ValidateError(HttpStatusCode.Conflict, "Email is already in use");
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
        Utils.Auth.AssertToken(response?.Token, JwtSettings, request.Email.ToLower());
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
        await outcome.ValidateError(HttpStatusCode.Unauthorized, "Invalid Credentials");
    }
}