using ConsiliumTempus.Domain.UserAggregate.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Mock;

public static class Mock
{
    public static class User
    {
        public static Domain.UserAggregate.User CreateMock(string email = "", string password = "")
        {
            return Domain.UserAggregate.User.Create(Credentials.Create(email, password), Name.Create("", ""));
        }
    }
}