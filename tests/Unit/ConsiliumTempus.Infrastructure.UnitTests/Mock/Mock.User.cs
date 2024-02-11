using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Infrastructure.UnitTests.Mock;

internal static partial class Mock
{
    internal static class User
    {
        public static UserAggregate CreateMock(
            Credentials credentials,
            Name name)
        {
            return UserAggregate.Create(credentials, name);
        }
    }
}