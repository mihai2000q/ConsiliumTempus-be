using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Delete;

[Collection(nameof(UserControllerCollection))]
public class UserControllerDeleteTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenDeleteUserIsSuccessful_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        const string email = "stephenc@gmail.com";
        
        // Act
        var id = Client.UseCustomToken(email);
        var outcome = await Client.Delete($"api/users");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(4);
        dbContext.Users.AsEnumerable()
            .SingleOrDefault(u => u.Id == id)
            .Should().BeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteUserResponse>();
        response!.Message.Should().Be("User has been deleted successfully!");
    }
}