using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Login;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerLoginValidationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Auth", false)
{
    [Fact]
    public async Task Login_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(
            email: "MichaelJ@Gmail.com",
            password: "MichaelJordan2");

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateLoginRequest(email: "no email");

        // Act
        var outcome = await Client.Post("/api/auth/Login", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}