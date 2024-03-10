using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerValidationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Auth", false)
{
    [Fact]
    public async Task Register_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest(
            firstName: "First", 
            lastName: "Last", 
            email: "FirstLast@Example.com");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRegisterRequest(
            firstName: string.Empty,
            lastName: string.Empty,
            email: "some wrong email");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Register", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }

    [Fact]
    public async Task Login_WhenQueryIsValid_ShouldReturnToken()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(
            email: "MichaelJ@Gmail.com",
            password: "MichaelJordan2");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WhenQueryIsValid_ShouldReturnInvalidCredentialsError()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(email: "no email");

        // Act
        var outcome = await Client.PostAsJsonAsync("/api/auth/Login", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}