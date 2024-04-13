using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.User.Events;

public sealed record UserDeleted(UserAggregate User) : IDomainEvent;