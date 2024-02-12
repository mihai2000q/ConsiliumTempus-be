using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.User.Commands.Update;
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
        
        internal static bool AssertUpdateCommand(UpdateUserCommand command, UpdateUserRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.FirstName.Should().Be(request.FirstName);
            command.LastName.Should().Be(request.LastName);
            command.Role.Should().Be(request.Role);
            command.DateOfBirth.Should().Be(request.DateOfBirth);
            return true;
        }
        
        internal static void AssertDto(IActionResult outcome, UserAggregate user)
        {
            outcome.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)outcome).Value.Should().BeOfType<UserDto>();

            var response = ((OkObjectResult)outcome).Value as UserDto;

            AssertDto(response!, user);
        }
        
        private static void AssertDto(UserDto dto, UserAggregate user)
        {
            dto.Id.Should().Be(user.Id.Value.ToString());
            dto.FirstName.Should().Be(user.Name.First);
            dto.LastName.Should().Be(user.Name.Last);
            dto.Email.Should().Be(user.Credentials.Email);
            dto.Role.Should().Be(user.Role);
            dto.DateOfBirth.Should().Be(user.DateOfBirth);
        }
    }
}