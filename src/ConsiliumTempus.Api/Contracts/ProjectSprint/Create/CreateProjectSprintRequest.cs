namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Create;

public sealed record CreateProjectSprintRequest(
    Guid ProjectId,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate);