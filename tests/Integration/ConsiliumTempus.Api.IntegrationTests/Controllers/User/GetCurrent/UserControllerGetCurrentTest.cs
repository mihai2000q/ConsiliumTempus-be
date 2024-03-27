using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.GetCurrent;

[Collection(nameof(UserControllerCollection))]
public class UserControllerGetCurrentTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task WhenGetCurrentUserIsSuccessful_ThenReturnCurrentUser()
    {
        // Arrange
        const string email = "michaelj@gmail.com";
        
        // Act
        Client.UseCustomToken(email);
        var outcome = await Client.Get("api/users/current");

        // Assert
        await Utils.User.AssertDtoFromResponse(
            outcome, 
            "Michael", 
            "Jordan", 
            email,
            "Pro Basketball Player",
            DateOnly.Parse("2000-12-23"));
    }
}