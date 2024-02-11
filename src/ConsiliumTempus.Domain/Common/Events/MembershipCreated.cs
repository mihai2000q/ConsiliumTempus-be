using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Events;

public sealed record MembershipCreated(Membership Membership) : IDomainEvent;