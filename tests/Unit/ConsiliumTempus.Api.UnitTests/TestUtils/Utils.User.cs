using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetCurrent;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Domain.User;
using Microsoft.AspNetCore.Mvc;

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
        
        internal static void AssertGetUser(IActionResult response, UserAggregate user)
        {
            response.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)response).Value.Should().BeOfType<GetUserResponse>();

            var userResponse = ((OkObjectResult)response).Value as GetUserResponse;
            
            userResponse!.FirstName.Should().Be(user.FirstName.Value);
            userResponse.LastName.Should().Be(user.LastName.Value);
            userResponse.Email.Should().Be(user.Credentials.Email);
            userResponse.Role.Should().Be(user.Role?.Value);
        }

        internal static void AssertGetCurrentUser(IActionResult response, UserAggregate user)
        {
            response.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)response).Value.Should().BeOfType<GetCurrentUserResponse>();

            var userResponse = ((OkObjectResult)response).Value as GetCurrentUserResponse;
            
            userResponse!.FirstName.Should().Be(user.FirstName.Value);
            userResponse.LastName.Should().Be(user.LastName.Value);
            userResponse.Email.Should().Be(user.Credentials.Email);
            userResponse.Role.Should().Be(user.Role?.Value);
            userResponse.DateOfBirth.Should().Be(user.DateOfBirth);
        }
    }
}