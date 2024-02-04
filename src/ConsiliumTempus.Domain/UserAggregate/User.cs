using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.UserAggregate.ValueObjects;

namespace ConsiliumTempus.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId, Guid>
{
    private User()
    {
    }
    
    private User(
        UserId id,
        string email,
        string password,
        string firstName,
        string lastName,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    public static User Create(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        return new User(
            UserId.CreateUnique(),
            email,
            password,
            firstName,
            lastName,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}