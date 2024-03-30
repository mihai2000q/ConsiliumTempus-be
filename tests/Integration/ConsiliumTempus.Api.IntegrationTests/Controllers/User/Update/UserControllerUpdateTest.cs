using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

[Collection(nameof(UserControllerCollection))]
public class UserControllerUpdateTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task UpdateUser_WhenIsSuccessful_ShouldUpdateAndReturnNewUser()
    {
        // Arrange
        var user = UserData.Users.First();
        var request = UserRequestFactory.CreateUpdateUserRequest(role: "Software Developer");

        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var dbContext = await DbContextFactory.CreateDbContextAsync();
        var updatedUser = await dbContext.Users.FindAsync(user.Id);
        Utils.User.AssertUpdate(updatedUser!, request);

        await Utils.User.AssertDtoFromResponse(outcome, updatedUser!);
    }
}