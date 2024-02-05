using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.UserAggregate.Events;

public record UserRegistered(User User) : IDomainEvent;