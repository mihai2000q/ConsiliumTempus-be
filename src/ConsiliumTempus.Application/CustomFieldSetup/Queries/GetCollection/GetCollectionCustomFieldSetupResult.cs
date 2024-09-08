using ConsiliumTempus.Domain.CustomFieldSetup;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;

public sealed record GetCollectionCustomFieldSetupResult(
    List<CustomFieldSetupAggregate> CustomFieldSetups);