using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;

public sealed record GetCollectionCustomFieldSetupFromProjectRequest
{
    [FromRoute] public Guid ProjectId { get; init; }
}