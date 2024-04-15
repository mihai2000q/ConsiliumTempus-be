using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetCurrent;
using ConsiliumTempus.Api.Contracts.User.UpdateCurrent;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class User
    {
        internal static bool AssertGetQuery(GetUserQuery query, GetUserRequest request)
        {
            query.Id.Should().Be(request.Id);
            return true;
        }

        internal static bool AssertUpdateCommand(UpdateCurrentUserCommand command, UpdateCurrentUserRequest request)
        {
            command.FirstName.Should().Be(request.FirstName);
            command.LastName.Should().Be(request.LastName);
            command.Role.Should().Be(request.Role);
            command.DateOfBirth.Should().Be(request.DateOfBirth);
            return true;
        }

        internal static void AssertGetUserResponse(GetUserResponse response, UserAggregate user)
        {
            response.FirstName.Should().Be(user.FirstName.Value);
            response.LastName.Should().Be(user.LastName.Value);
            response.Email.Should().Be(user.Credentials.Email);
            response.Role.Should().Be(user.Role?.Value);
        }

        internal static void AssertGetCurrentUserResponse(GetCurrentUserResponse response, UserAggregate user)
        {
            response.FirstName.Should().Be(user.FirstName.Value);
            response.LastName.Should().Be(user.LastName.Value);
            response.Email.Should().Be(user.Credentials.Email);
            response.Role.Should().Be(user.Role?.Value);
            response.DateOfBirth.Should().Be(user.DateOfBirth);
        }
    }
}