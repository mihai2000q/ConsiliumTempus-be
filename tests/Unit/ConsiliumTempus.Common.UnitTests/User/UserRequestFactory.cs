using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.User;

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
        Guid? id = null,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new UpdateUserRequest(
            id ?? Guid.NewGuid(), 
            firstName, 
            lastName, 
            role, 
            dateOfBirth);
    }
}