using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.Contracts.User.Get;
using ConsiliumTempus.Api.Contracts.User.GetCurrent;
using ConsiliumTempus.Api.Contracts.User.UpdateCurrent;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class User
    {
        internal static void AssertGetResponse(
            GetUserResponse response,
            UserAggregate user)
        {
            response.FirstName.Should().Be(user.FirstName.Value);
            response.LastName.Should().Be(user.LastName.Value);
            response.Email.Should().Be(user.Credentials.Email);
            response.Role.Should().Be(user.Role?.Value);
        }
        
        internal static void AssertGetCurrentResponse(
            GetCurrentUserResponse response,
            UserAggregate user)
        {
            response.Id.Should().Be(user.Id.Value);
            response.FirstName.Should().Be(user.FirstName.Value);
            response.LastName.Should().Be(user.LastName.Value);
            response.Email.Should().Be(user.Credentials.Email);
            response.Role.Should().Be(user.Role?.Value);
            response.DateOfBirth.Should().Be(user.DateOfBirth);
        }

        internal static void AssertRegistration(
            UserAggregate user,
            RegisterRequest request)
        {
            user.Id.Value.Should().NotBeEmpty();
            user.FirstName.Value.Should().Be(request.FirstName.Capitalize());
            user.LastName.Value.Should().Be(request.LastName.Capitalize());
            user.Credentials.Email.Should().Be(request.Email.ToLower());
            user.Credentials.Password.Should().NotBeNullOrWhiteSpace().And.NotBe(request.Password);
            if (request.Role is null)
                user.Role.Should().BeNull();
            else
                user.Role!.Value.Should().Be(request.Role);
            user.DateOfBirth.Should().Be(request.DateOfBirth);
            user.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            user.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            user.Memberships.Should().HaveCount(1);
            user.Memberships[0].User.Should().Be(user);
            user.Memberships[0].Workspace.Name.Value.Should().Be(Constants.Workspace.Name);
            user.Memberships[0].Workspace.Description.Value.Should().Be(Constants.Workspace.Description);
            user.Memberships[0].Workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            user.Memberships[0].Workspace.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        internal static void AssertUpdate(
            UserAggregate user,
            UserAggregate newUser,
            UpdateCurrentUserRequest request)
        {
            // unchanged
            newUser.Id.Should().Be(user.Id);
            newUser.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            newUser.Credentials.Should().Be(user.Credentials);
            
            // changed
            newUser.FirstName.Value.Should().Be(request.FirstName.Capitalize());
            newUser.LastName.Value.Should().Be(request.LastName.Capitalize());
            if (request.Role is null)
                newUser.Role.Should().BeNull();
            else
                newUser.Role!.Value.Should().Be(request.Role);
            newUser.DateOfBirth.Should().Be(request.DateOfBirth);
            newUser.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
        }
    }
}