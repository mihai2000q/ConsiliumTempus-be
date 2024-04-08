using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.User;
using ConsiliumTempus.Domain.Common.Errors;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.UpdateCurrent;

[Collection(nameof(UserControllerCollection))]
public class UserControllerUpdateCurrentTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task UpdateCurrentUser_WhenIsSuccessful_ShouldUpdateAndReturnNewUser()
    {
        // Arrange
        var user = UserData.Users.First();
        var request = UserRequestFactory.CreateUpdateCurrentUserRequest(role: "Software Developer");

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/users/current", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await outcome.Content.ReadFromJsonAsync<UpdateCurrentUserResponse>();
        response!.Message.Should().Be("Current user has been updated successfully!");

        var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedUser = await dbContext.Users.FindAsync(user.Id);
        Utils.User.AssertUpdate(user, updatedUser!, request);
    }
    
    [Fact]
    public async Task UpdateCurrentUser_WhenItFails_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateCurrentUserRequest();

        // Act
        Client.UseInvalidToken();
        var outcome = await Client.Put("api/users/current", request);

        // Assert
        await outcome.ValidateError(Errors.User.NotFound);
    }
}