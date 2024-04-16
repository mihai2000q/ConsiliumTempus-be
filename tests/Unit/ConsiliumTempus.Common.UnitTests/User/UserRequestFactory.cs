using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.UpdateCurrent;
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

    public static UpdateCurrentUserRequest CreateUpdateCurrentUserRequest(
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new UpdateCurrentUserRequest(
            firstName,
            lastName,
            role,
            dateOfBirth);
    }
}