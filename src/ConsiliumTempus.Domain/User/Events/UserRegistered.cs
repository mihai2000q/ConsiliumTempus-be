using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.User.Events;

public record UserRegistered(UserAggregate User) : IDomainEvent;