using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.UnitTests.Mock;

internal static partial class Mock
{
    internal static class User
    {
        internal static UserAggregate CreateMock(
            string email = "Some@example.com",
            string password = "Password123")
        {
            return UserAggregate.Create(Credentials.Create(email, password), Name.Create("", ""));
        }
    }
}