using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Mock;

public static class Mock
{
    public static class User
    {
        public static UserAggregate CreateMock(string email = "", string password = "")
        {
            return UserAggregate.Create(Credentials.Create(email, password), Name.Create("", ""));
        }
    }
}