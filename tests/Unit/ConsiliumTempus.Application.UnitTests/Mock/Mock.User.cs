using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Mock;

public static partial class Mock
{
    public static class User
    {
        public static UserAggregate CreateMock(
            string email = "Some@example.com",
            string password = "Password123")
        {
            return UserAggregate.Create(Credentials.Create(email, password), Name.Create("", ""));
        }
    }
}