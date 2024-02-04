namespace ConsiliumTempus.Infrastructure.UnitTests.Mock;

public static class Mock
{
    public static class User
    {
        public static Domain.UserAggregate.User CreateMock(
            string email = "", 
            string password = "",
            string firstName = "",
            string lastName = "")
        {
            return Domain.UserAggregate.User.Create(email, password, firstName, lastName);
        }
    }
}