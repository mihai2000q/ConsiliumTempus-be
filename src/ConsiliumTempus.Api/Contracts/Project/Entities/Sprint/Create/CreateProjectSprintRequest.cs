namespace ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;

public sealed record CreateProjectSprintRequest(
    Guid ProjectId,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate);