using ConsiliumTempus.Application.Authentication.Commands.Register;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.User.ValueObjects;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class User
    {
        internal static bool AssertId(UserId userId, string id)
        {
            userId.Should().Be(UserId.Create(id));
            return true;
        }

        internal static void AssertFromRegisterCommand(UserAggregate user, RegisterCommand command, string password)
        {
            user.Id.Should().NotBeNull();
            user.Name.First.Should().Be(command.FirstName.CapitalizeWord());
            user.Name.Last.Should().Be(command.LastName.CapitalizeWord());
            user.Credentials.Email.Should().Be(command.Email.ToLower());
            user.Credentials.Password.Should().Be(password);
            user.Role.Should().Be(command.Role);
            user.DateOfBirth.Should().Be(command.DateOfBirth);
            user.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());
            user.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, 1.Minutes());

            user.DomainEvents.Should().HaveCount(1);
            user.DomainEvents[0].Should().BeOfType<UserRegistered>();
            ((UserRegistered)user.DomainEvents[0]).User.Should().Be(user);
        }

        internal static void AssertFromUpdateCommand(
            UpdateUserResult result,
            UpdateUserCommand command)
        {
            result.User.Id.Value.Should().Be(command.Id);
            result.User.Name.First.Should().Be(command.FirstName);
            result.User.Name.Last.Should().Be(command.LastName);
            result.User.Role.Should().Be(command.Role);
            result.User.DateOfBirth.Should().Be(command.DateOfBirth);
            result.User.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        internal static void AssertUser(UserAggregate outcome, UserAggregate expected)
        {
            outcome.Id.Should().Be(expected.Id);
            outcome.Name.Should().Be(expected.Name);
            outcome.Credentials.Should().Be(expected.Credentials);
            outcome.Role.Should().Be(expected.Role);
            outcome.DateOfBirth.Should().Be(expected.DateOfBirth);
            outcome.CreatedDateTime.Should().BeCloseTo(expected.CreatedDateTime, TimeSpan.FromMinutes(1));
            outcome.UpdatedDateTime.Should().BeCloseTo(expected.UpdatedDateTime, TimeSpan.FromMinutes(1));
            outcome.Memberships.Should().BeEquivalentTo(expected.Memberships);
        }
    }
}