using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Common.IntegrationTests.User;

public static class UserFactory
{
    public static UserAggregate Create(
        string email,
        string password,
        string firstName,
        string lastName,
        string? role = null,
        DateOnly? dateOfBirth = null,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        return EntityBuilder<UserAggregate>.Empty()
            .WithProperty(nameof(UserAggregate.Id), UserId.CreateUnique())
            .WithProperty(nameof(UserAggregate.Credentials), Credentials.Create(email, password))
            .WithProperty(nameof(UserAggregate.FirstName), FirstName.Create(firstName))
            .WithProperty(nameof(UserAggregate.LastName), LastName.Create(lastName))
            .WithProperty(nameof(UserAggregate.Role), role is null ? null : Role.Create(role))
            .WithProperty(nameof(UserAggregate.DateOfBirth), dateOfBirth)
            .WithProperty(nameof(UserAggregate.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(UserAggregate.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow)
            .Build();
    }
}