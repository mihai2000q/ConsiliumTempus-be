using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.Authentication;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.Auth.Register;

[Collection(nameof(AuthenticationControllerCollection))]
public class AuthenticationControllerRegisterValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new AuthData(), true)
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
        var outcome = await Client.Post("/api/auth/Register", request);

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
        var outcome = await Client.Post("/api/auth/Register", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}