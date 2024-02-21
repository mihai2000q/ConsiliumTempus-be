using ConsiliumTempus.Application.Authentication.Queries.Login;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Authentication;

public static class AuthenticationQueryFactory
{
    public static LoginQuery CreateLoginQuery(
        string email = Constants.User.Email, 
        string password = Constants.User.Password)
    {
        return new LoginQuery(email, password);
    }
}