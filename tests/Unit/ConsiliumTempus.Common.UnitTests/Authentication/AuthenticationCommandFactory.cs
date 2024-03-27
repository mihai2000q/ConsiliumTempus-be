using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Authentication;

public static class AuthenticationCommandFactory
{
    public static RegisterCommand CreateRegisterCommand(
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string email = Constants.User.Email,
        string password = Constants.User.Password,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new RegisterCommand(
            firstName, 
            lastName, 
            email, 
            password, 
            role, 
            dateOfBirth);
    }
    
    public static LoginCommand CreateLoginCommand(
        string email = Constants.User.Email, 
        string password = Constants.User.Password)
    {
        return new LoginCommand(email, password);
    }
}