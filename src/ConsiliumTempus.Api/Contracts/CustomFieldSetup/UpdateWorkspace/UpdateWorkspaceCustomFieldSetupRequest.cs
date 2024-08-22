namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.UpdateWorkspace;

public sealed record UpdateWorkspaceCustomFieldSetupRequest(
    Guid Id,
    Guid WorkspaceId);