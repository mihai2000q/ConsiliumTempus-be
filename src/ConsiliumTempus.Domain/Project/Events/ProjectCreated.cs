using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Project.Events;

public sealed record ProjectCreated(
    ProjectAggregate Project,
    UserAggregate User) : IDomainEvent;