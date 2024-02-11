using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.User.Events;

public sealed record UserRegistered(UserAggregate User) : IDomainEvent;