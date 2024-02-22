using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth;

public class AuthenticationControllerValidationTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Auth", false)
{
    [Fact]
    public async Task Register_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new RegisterRequest(
            "First",
            "Last",
            "FirstLast@Example.com",
            "Password123",
            null,
            null);

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = new RegisterRequest(
            "",
            "",
            "Some wrong Email",
            "short",
            null,
            null);

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }

    [Fact]
    public async Task Login_WhenQueryIsValid_ShouldReturnToken()
    {
        // Arrange
        var request = new LoginRequest(
            "MichaelJ@Gmail.com",
            "MichaelJordan2");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WhenQueryIsValid_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var request = new LoginRequest(
            "no email",
            "no pas");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}