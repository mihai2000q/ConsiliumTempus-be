using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Authentication;

public static class AuthenticationResultFactory
{
    public static RegisterResult CreateRegisterResult(
        string token = Constants.Auth.Token,
        Guid? refreshToken = null)
    {
        return new RegisterResult(
            token,
            refreshToken ?? Guid.NewGuid());
    }
    
    public static LoginResult CreateLoginResult(
        string token = Constants.Auth.Token,
        Guid? refreshToken = null)
    {
        return new LoginResult(
            token,
            refreshToken ?? Guid.NewGuid());
    }
    
    public static RefreshResult CreateRefreshResult(string token = Constants.Auth.Token)
    {
        return new RefreshResult(token);
    }
}