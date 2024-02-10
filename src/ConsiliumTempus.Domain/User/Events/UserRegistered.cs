using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.User.Events;

public record UserRegistered(UserAggregate User) : IDomainEvent;