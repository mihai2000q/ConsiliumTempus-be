using ConsiliumTempus.Application.User.Commands.Update;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class User
    {
        internal static bool AssertUserId(UserId userId, string id)
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
            result.User.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }
    }
}