using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Api.IntegrationTests.TestFactory.Request;

public static class UserRequestFactory
{
    public static GetUserRequest CreateGetUserRequest(
        Guid? id = null)
    {
        return new GetUserRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }
    
    public static UpdateUserRequest CreateUpdateUserRequest(
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new UpdateUserRequest(
            firstName, 
            lastName, 
            role, 
            dateOfBirth);
    }
}