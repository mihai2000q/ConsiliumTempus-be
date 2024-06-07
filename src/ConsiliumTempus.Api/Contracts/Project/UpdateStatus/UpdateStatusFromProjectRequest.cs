namespace ConsiliumTempus.Api.Contracts.Project.UpdateStatus;

public sealed record UpdateStatusFromProjectRequest(
    Guid Id,
    Guid StatusId,
    string Title,
    string Status,
    string Description);