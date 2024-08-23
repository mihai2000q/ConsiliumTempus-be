using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.RemoveFromProject;

public sealed record RemoveCustomFieldSetupFromProjectRequest()
{
    [FromRoute] public Guid Id { get; init; }
    [FromRoute] public Guid ProjectId { get; init; }
}