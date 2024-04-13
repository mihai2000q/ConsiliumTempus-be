using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Common.UnitTests.User;

public static class UserFactory
{
    public static UserAggregate Create(
        string email = Constants.User.Email,
        string password = Constants.User.Password,
        string firstName = Constants.User.FirstName,
        string lastName = Constants.User.LastName,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        var user = UserAggregate.Register(
            Credentials.Create(email, password), 
            FirstName.Create(firstName),
            LastName.Create(lastName),
            role is null ? null : Role.Create(role),
            dateOfBirth);
        user.ClearDomainEvents();
        return user;
    }

    public static UserId CreateId(Guid? id = null)
    {
        return UserId.Create(id ?? Guid.NewGuid());
    }
}