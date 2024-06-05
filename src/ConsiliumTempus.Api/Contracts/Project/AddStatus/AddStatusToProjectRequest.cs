namespace ConsiliumTempus.Api.Contracts.Project.AddStatus;

public sealed record AddStatusToProjectRequest(
    Guid Id,
    string Title,
    string Status,
    string Description);