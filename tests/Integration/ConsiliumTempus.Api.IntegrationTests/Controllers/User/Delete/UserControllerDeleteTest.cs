using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Delete;

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
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(4);
        dbContext.Users.AsEnumerable()
            .SingleOrDefault(u => u.Id.Value.ToString() == id)
            .Should().BeNull();
        
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<DeleteUserResponse>();
        response!.Message.Should().Be("User has been deleted successfully!");
    }
    
    [Fact]
    public async Task WhenDeleteUserIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        const string id = "90000000-0000-0000-0000-000000000000";
        
        // Act
        var outcome = await Client.DeleteAsync($"api/users/{id}");

        // Assert
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(5);
        dbContext.Users.AsEnumerable().SingleOrDefault(u => u.Id.Value.ToString() == id).Should().BeNull();
        
        await outcome.ValidateError(Errors.User.NotFound);
    }
}