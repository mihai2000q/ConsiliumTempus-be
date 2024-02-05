using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

public static partial class Utils
{
    public static class User
    {
        public static bool AssertUserId(UserId userId, string id)
        {
            userId.Should().Be(UserId.Create(new Guid(id)));
            return true;
        }
    }
}