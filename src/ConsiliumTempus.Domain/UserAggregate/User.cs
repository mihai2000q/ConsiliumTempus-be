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
        Credentials credentials,
        Name name,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Credentials = credentials;
        Name = name;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }


    public Credentials Credentials { get; private set; }
    public Name Name { get; private set; } 
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    public static User Create(
        Credentials credentials,
        Name name)
    {
        return new User(
            UserId.CreateUnique(),
            credentials,
            name,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}