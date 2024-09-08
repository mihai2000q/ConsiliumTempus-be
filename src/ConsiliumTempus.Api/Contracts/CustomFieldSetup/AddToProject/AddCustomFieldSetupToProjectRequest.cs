namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.AddToProject;

public sealed record AddCustomFieldSetupToProjectRequest(
    Guid Id,
    Guid ProjectId);