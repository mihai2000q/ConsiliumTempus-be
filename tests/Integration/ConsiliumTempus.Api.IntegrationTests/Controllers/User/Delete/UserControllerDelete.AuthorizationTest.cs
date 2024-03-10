using System.Net;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Delete;

[Collection(nameof(UserControllerCollection))]
public class UserControllerDeleteAuthorizationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task DeleteUser_WhenUserIsOwner_ShouldReturnSuccessResponse()
    {
        // Arrange
        const string email = "stephenc@gmail.com";
        const string id = "40000000-0000-0000-0000-000000000000";
        
        // Act
        Client.UseCustomToken(email);
        var outcome = await Client.Delete($"api/users/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteUser_WhenUserIsNotOwner_ShouldReturnForbiddenResponse()
    {
        // Arrange
        const string email = "stephenc@gmail.com";
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        Client.UseCustomToken(email);
        var outcome = await Client.Delete($"api/users/{id}");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}