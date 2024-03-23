using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

[Collection(nameof(UserControllerCollection))]
public class UserControllerUpdateTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task UpdateUser_WhenIsSuccessful_ShouldUpdateAndReturnNewUser()
    {
        // Arrange
        const string email = "michaelj@gmail.com";
        var request = UserRequestFactory.CreateUpdateUserRequest(role: "Software Developer");
        
        // Act
        var id = Client.UseCustomToken(email);
        var outcome = await Client.Put("api/users", request);

        // Assert
        var updatedUser = await GetUserById(id);
        Utils.User.AssertUpdate(updatedUser!, request);
        
        await Utils.User.AssertDtoFromResponse(
            outcome,
            request.FirstName, 
            request.LastName, 
            email, 
            request.Role,
            request.DateOfBirth);
    }

    private async Task<UserAggregate?> GetUserById(UserId id)
    {
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }
}