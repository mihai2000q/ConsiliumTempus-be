using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Api.IntegrationTests.TestFactory.Domain;

internal static class UserFactory
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
        var user = DomainFactory.GetObjectInstance<UserAggregate>();

        DomainFactory.SetProperty(ref user, nameof(user.Id), UserId.CreateUnique());
        DomainFactory.SetProperty(ref user, nameof(user.Credentials), Credentials.Create(email, password));
        DomainFactory.SetProperty(ref user, nameof(user.FirstName), FirstName.Create(firstName));
        DomainFactory.SetProperty(ref user, nameof(user.LastName), LastName.Create(lastName));
        DomainFactory.SetProperty(ref user, nameof(user.Role), role is null ? null : Role.Create(role));
        DomainFactory.SetProperty(ref user, nameof(user.DateOfBirth), dateOfBirth);
        DomainFactory.SetProperty(ref user, nameof(user.CreatedDateTime), createdDateTime ?? DateTime.UtcNow);
        DomainFactory.SetProperty(ref user, nameof(user.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow);
        
        return user;
    }
}