using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.GetId;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.GetId;

[Collection(nameof(UserControllerCollection))]
public class UserControllerGetIdTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenGetUserIdIsSuccessful_ThenReturnTheUserId()
    {
        // Arrange
        
        // Act
        var outcome = await Client.Get($"api/users/id");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await outcome.Content.ReadFromJsonAsync<GetUserIdResponse>();
        content?.Id.Should().NotBeNullOrWhiteSpace().And.HaveLength(36);
    }
}