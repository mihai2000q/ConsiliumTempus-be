using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Common.UnitTests.TestConstants;

namespace ConsiliumTempus.Common.UnitTests.Authentication;

public static class AuthenticationRequestFactory
{
    public static RegisterRequest CreateRegisterRequest(
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string email = Constants.User.Email,
        string password = Constants.User.Password,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new RegisterRequest(
            firstName,
            lastName,
            email,
            password,
            role,
            dateOfBirth);
    }

    public static LoginRequest CreateLoginRequest(
        string email = Constants.User.Email,
        string password = Constants.User.Password)
    {
        return new LoginRequest(email, password);
    }

    public static RefreshRequest CreateRefreshRequest(
        string token = Constants.Auth.Token,
        Guid? refreshToken = null)
    {
        return new RefreshRequest(token, refreshToken ?? Guid.NewGuid());
    }
}