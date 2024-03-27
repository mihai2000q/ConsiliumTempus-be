using ConsiliumTempus.Api.Contracts.Authentication.Login;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Application.Authentication.Commands.Login;
using ConsiliumTempus.Application.Authentication.Commands.Register;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Authentication
    {
        public static bool AssertRegisterCommand(RegisterCommand command, RegisterRequest request)
        {
            command.FirstName.Should().Be(request.FirstName);
            command.LastName.Should().Be(request.LastName);
            command.Email.Should().Be(request.Email);
            command.Password.Should().Be(request.Password);
            command.Role.Should().Be(request.Role);
            command.DateOfBirth.Should().Be(request.DateOfBirth);
            return true;
        }

        public static bool AssertLoginQuery(LoginCommand command, LoginRequest request)
        {
            command.Email.Should().Be(request.Email);
            command.Password.Should().Be(request.Password);
            return true;
        }
    }
}