using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;

public sealed record GetCollectionCustomFieldSetupFromWorkspaceRequest
{
    [FromRoute] public Guid WorkspaceId { get; init; }
}