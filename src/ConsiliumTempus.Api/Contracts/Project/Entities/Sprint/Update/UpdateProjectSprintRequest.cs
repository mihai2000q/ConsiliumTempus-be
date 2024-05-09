namespace ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Update;

public sealed record UpdateProjectSprintRequest(
    Guid Id,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate);