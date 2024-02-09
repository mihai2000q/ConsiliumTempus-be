using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Authentication.Queries.Login;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

public static partial class Utils
{
    public static class Authentication
    {
        public static bool AssertRegisterCommand(RegisterCommand command, RegisterRequest request)
        {
            command.FirstName.Should().Be(request.FirstName);
            command.LastName.Should().Be(request.LastName);
            command.Email.Should().Be(request.Email);
            command.Password.Should().Be(request.Password);
            return true;
        }

        public static bool AssertLoginQuery(LoginQuery query, LoginRequest request)
        {
            query.Email.Should().Be(request.Email);
            query.Password.Should().Be(request.Password);
            return true;
        }
    }
}