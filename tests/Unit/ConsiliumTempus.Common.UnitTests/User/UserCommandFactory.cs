using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.User;

public static class UserCommandFactory
{
    public static UpdateUserCommand CreateUpdateUserCommand(
        Guid? id = null,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new UpdateUserCommand(
            id ?? Guid.NewGuid(), 
            firstName, 
            lastName, 
            role, 
            dateOfBirth);
    }
}