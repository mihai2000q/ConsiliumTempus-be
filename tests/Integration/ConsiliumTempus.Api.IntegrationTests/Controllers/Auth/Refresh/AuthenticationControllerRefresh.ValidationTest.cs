using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Refresh;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerRefreshValidationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest(factory, testOutputHelper, "Auth/Refresh", false)
{
    [Fact]
    public async Task Refresh_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest();

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Refresh_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = AuthenticationRequestFactory.CreateRefreshRequest(token: "");

        // Act
        var outcome = await Client.Post("/api/auth/Refresh", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}