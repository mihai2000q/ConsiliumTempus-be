using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

[Collection(nameof(UserControllerCollection))]
public class UserControllerUpdateAuthorizationTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task UpdateUser_WhenUserIsOwner_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest(
            id: new Guid("10000000-0000-0000-0000-000000000000"));
        
        // Act
        UseCustomToken("michaelj@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateUser_WhenUserIsNotOwner_ShouldReturnForbiddenResponse()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest(
            id: new Guid("10000000-0000-0000-0000-000000000000"));
        
        // Act
        UseCustomToken("stephenc@gmail.com");
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}