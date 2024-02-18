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
        string? role,
        DateOnly? dateOfBirth,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Credentials = credentials;
        Name = name;
        Role = role;
        DateOfBirth = dateOfBirth;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<Membership> _memberships = [];

    public Credentials Credentials { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
    public string? Role { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();

    public static UserAggregate Create(
        Credentials credentials,
        Name name,
        string? role = null,
        DateOnly? dateOfBirth = null)
    {
        return new UserAggregate(
            UserId.CreateUnique(),
            credentials,
            name,
            role,
            dateOfBirth,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
    
    public static UserAggregate Register(
        Credentials credentials,
        Name name,
        string? role,
        DateOnly? dateOfBirth)
    {
        var user = Create(credentials, name, role, dateOfBirth);

        user.AddDomainEvent(new UserRegistered(user));
        
        return user;
    }
    
    public void Update(
        Name name,
        string? role,
        DateOnly? dateOfBirth)
    {
        Name = name;
        Role = role;
        DateOfBirth = dateOfBirth;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddWorkspaceMembership(Membership membership)
    {
        _memberships.Add(membership);
    }
}