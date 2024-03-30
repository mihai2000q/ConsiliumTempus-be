using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.User;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

[Collection(nameof(UserControllerCollection))]
public class UserControllerUpdateValidationTest(WebAppFactory factory) 
    : BaseIntegrationTest(factory, new UserData())
{
    [Fact]
    public async Task UpdateUser_WhenCommandIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest();
        
        // Act
        var outcome = await Client.Put("api/users", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateUser_WhenCommandIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest(
            firstName: string.Empty, 
            role: new string('a', 1000));
        
        // Act
        var outcome = await Client.Put("api/users", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}