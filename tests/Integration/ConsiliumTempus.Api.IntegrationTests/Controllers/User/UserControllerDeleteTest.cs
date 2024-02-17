using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User;

public class UserControllerDeleteTest(
    ConsiliumTempusWebApplicationFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenDeleteUserIsSuccessful_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        const string email = "stephenc@gmail.com";
        const string id = "40000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/users/{id}");

        // Assert
        DbContext.Users.Should().HaveCount(4);
        DbContext.Users.AsEnumerable()
            .SingleOrDefault(u => u.Id.Value.ToString() == id)
            .Should().BeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteUserResponse>();
        response!.Message.Should().Be("User has been deleted successfully!");
    }
    
    [Fact]
    public async Task WhenDeleteUserIsNotOwner_ShouldReturnForbiddenResponse()
    {
        // Arrange
        const string email = "stephenc@gmail.com";
        const string id = "10000000-0000-0000-0000-000000000000";
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.DeleteAsync($"api/users/{id}");

        // Assert
        DbContext.Users.Should().HaveCount(5);
        DbContext.Users.AsEnumerable()
            .SingleOrDefault(u => u.Id.Value.ToString() == id)
            .Should().NotBeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task WhenDeleteUserIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "90000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/users/{id}");

        // Assert
        DbContext.Users.Should().HaveCount(5);
        DbContext.Users.AsEnumerable().SingleOrDefault(u => u.Id.Value.ToString() == id).Should().BeNull();
        
        await outcome.ValidateError(HttpStatusCode.NotFound, Errors.User.NotFound.Description);
    }
}