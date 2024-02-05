using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Infrastructure.UnitTests.Mock;

public static class Mock
{
    public static class User
    {
        public static UserAggregate CreateMock(
            Credentials credentials,
            Name name)
        {
            return UserAggregate.Create(credentials, name);
        }
    }
}