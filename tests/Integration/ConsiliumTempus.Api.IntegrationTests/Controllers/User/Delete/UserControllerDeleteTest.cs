using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Delete;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Delete;

[Collection(nameof(UserControllerCollection))]
public class UserControllerDeleteTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task WhenDeleteUserIsSuccessful_ShouldDeleteAndReturnSuccessResponse()
    {
        // Arrange
        var user = UserData.Users.First();
        
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Delete("api/users");

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await outcome.Content.ReadFromJsonAsync<DeleteUserResponse>();
        response!.Message.Should().Be("User has been deleted successfully!");
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(UserData.Users.Length - 1);
        (await dbContext.Users.FindAsync(user.Id))
            .Should().BeNull();
    }
    
    [Fact]
    public async Task WhenDeleteUserFails_ShouldReturnNotFoundError()
    {
        // Arrange
        
        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Delete("api/users");

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
        
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Users.Should().HaveCount(UserData.Users.Length);
    }
}