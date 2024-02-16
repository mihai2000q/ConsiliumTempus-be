using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

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