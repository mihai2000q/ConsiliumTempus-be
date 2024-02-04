using ConsiliumTempus.Domain.UserAggregate.ValueObjects;

namespace ConsiliumTempus.Infrastructure.UnitTests.Mock;

public static class Mock
{
    public static class User
    {
        public static Domain.UserAggregate.User CreateMock(
            Credentials credentials,
            Name name)
        {
            return Domain.UserAggregate.User.Create(credentials, name);
        }
    }
}