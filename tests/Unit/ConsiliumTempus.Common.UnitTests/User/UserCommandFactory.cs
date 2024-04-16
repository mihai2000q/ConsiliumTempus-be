using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.User;

public static class UserCommandFactory
{
    public static UpdateCurrentUserCommand CreateUpdateUserCommand(
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new UpdateCurrentUserCommand(
            firstName,
            lastName,
            role,
            dateOfBirth);
    }
}