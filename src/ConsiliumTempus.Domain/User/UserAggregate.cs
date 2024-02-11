using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Domain.User;

public sealed class UserAggregate : AggregateRoot<UserId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private UserAggregate()
    {
    }
    
    private UserAggregate(
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

    private readonly List<Membership> _memberships = [];

    public Credentials Credentials { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();

    public static UserAggregate Create(
        Credentials credentials,
        Name name)
    {
        return new UserAggregate(
            UserId.CreateUnique(),
            credentials,
            name,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
    
    public static UserAggregate Register(
        Credentials credentials,
        Name name)
    {
        var user = Create(credentials, name);

        user.AddDomainEvent(new UserRegistered(user));
        
        return user;
    }
    
    public void Update(Name name)
    {
        Name = name;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddWorkspaceMembership(Membership membership)
    {
        _memberships.Add(membership);
    }
}