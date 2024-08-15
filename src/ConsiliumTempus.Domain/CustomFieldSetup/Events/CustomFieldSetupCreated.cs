using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Events;

public sealed record CustomFieldSetupCreated(CustomFieldSetupAggregate CustomFieldSetup) : IDomainEvent;