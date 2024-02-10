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
    }
}