using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User.Events;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Domain.User;

public sealed class UserAggregate : AggregateRoot<UserId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private UserAggregate()
    {
    }

    private UserAggregate(
        UserId id,
        Credentials credentials,
        FirstName firstName,
        LastName lastName,
        Role? role,
        DateOnly? dateOfBirth,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Credentials = credentials;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        DateOfBirth = dateOfBirth;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<Membership> _memberships = [];

    public Credentials Credentials { get; private set; } = default!;
    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public Name Name => Name.Create(FirstName.Value + " " + LastName.Value);
    public Role? Role { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public IReadOnlyList<Membership> Memberships => _memberships.AsReadOnly();
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }

    public static UserAggregate Register(
        Credentials credentials,
        FirstName firstName,
        LastName lastName,
        Role? role,
        DateOnly? dateOfBirth)
    {
        var user = new UserAggregate(
            UserId.CreateUnique(),
            credentials,
            firstName,
            lastName,
            role,
            dateOfBirth,
            DateTime.UtcNow,
            DateTime.UtcNow);

        user.AddDomainEvent(new UserRegistered(user));

        return user;
    }

    public void Update(
        FirstName firstName,
        LastName lastName,
        Role? role,
        DateOnly? dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        DateOfBirth = dateOfBirth;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddWorkspaceMembership(Membership membership)
    {
        _memberships.Add(membership);
    }
}