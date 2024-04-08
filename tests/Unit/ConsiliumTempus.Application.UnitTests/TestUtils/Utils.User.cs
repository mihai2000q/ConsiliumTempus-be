using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.User.Commands.UpdateCurrent;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.Events;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class User
    { 
        internal static void AssertFromRegisterCommand(UserAggregate user, RegisterCommand command, string password)
        {
            user.Id.Should().NotBeNull();
            user.FirstName.Value.Should().Be(command.FirstName.CapitalizeWord());
            user.LastName.Value.Should().Be(command.LastName.CapitalizeWord());
            user.Credentials.Email.Should().Be(command.Email.ToLower());
            user.Credentials.Password.Should().Be(password);
            if (command.Role is null) 
                user.Role.Should().BeNull();
            else
                user.Role!.Value.Should().Be(command.Role);
            user.DateOfBirth.Should().Be(command.DateOfBirth);
            user.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            user.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            user.DomainEvents.Should().HaveCount(1);
            user.DomainEvents[0].Should().BeOfType<UserRegistered>();
            ((UserRegistered)user.DomainEvents[0]).User.Should().Be(user);
        }

        internal static void AssertUpdate(
            UserAggregate user,
            UpdateCurrentUserCommand command)
        {
            user.FirstName.Value.Should().Be(command.FirstName);
            user.LastName.Value.Should().Be(command.LastName);
            if (command.Role is null) 
                user.Role.Should().BeNull();
            else
                user.Role!.Value.Should().Be(command.Role);
            user.DateOfBirth.Should().Be(command.DateOfBirth);
            user.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        internal static void AssertUser(UserAggregate outcome, UserAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Credentials.Should().Be(expected.Credentials);
            outcome.FirstName.Should().Be(expected.FirstName);
            outcome.LastName.Should().Be(expected.LastName);
            outcome.Role.Should().Be(expected.Role);
            outcome.DateOfBirth.Should().Be(expected.DateOfBirth);
            outcome.CreatedDateTime.Should().BeCloseTo(expected.CreatedDateTime, TimeSpan.FromMinutes(1));
            outcome.UpdatedDateTime.Should().BeCloseTo(expected.UpdatedDateTime, TimeSpan.FromMinutes(1));
            outcome.Memberships.Should().BeEquivalentTo(expected.Memberships);
        }
    }
}