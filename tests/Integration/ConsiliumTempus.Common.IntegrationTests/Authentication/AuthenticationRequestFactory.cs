using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Refresh;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;

namespace ConsiliumTempus.Common.IntegrationTests.Authentication;

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
        string refreshToken,
        string token)
    {
        return new RefreshRequest(token, refreshToken);
    }
}