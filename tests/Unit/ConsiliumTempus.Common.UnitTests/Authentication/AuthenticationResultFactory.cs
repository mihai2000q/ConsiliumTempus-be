using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Authentication.Commands.Refresh;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Authentication;

public static class AuthenticationResultFactory
{
    public static RegisterResult CreateRegisterResult(
        string token = Constants.Auth.Token,
        string refreshToken = Constants.Auth.RefreshToken)
    {
        return new RegisterResult(
            token,
            refreshToken);
    }
    
    public static LoginResult CreateLoginResult(
        string token = Constants.Auth.Token,
        string refreshToken = Constants.Auth.RefreshToken)
    {
        return new LoginResult(
            token,
            refreshToken);
    }
    
    public static RefreshResult CreateRefreshResult(string token = Constants.Auth.Token)
    {
        return new RefreshResult(token);
    }
}