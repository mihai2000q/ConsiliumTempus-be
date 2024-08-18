using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Domain.CustomFieldSetup.Events;

public sealed record RemovedCustomFieldSetupFromProject(
    CustomFieldSetupAggregate CustomFieldSetup, 
    ProjectAggregate Project) 
    : IDomainEvent;