using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.CustomFieldSetup.Variants;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Events;

public sealed record RemovedOptionFromSingleSelectCustomFieldSetup(
    SingleSelectCustomFieldSetupAggregate CustomFieldSetup,
    SingleSelectOption Option) : IDomainEvent;
