namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Update;

public sealed record UpdateProjectSprintRequest(
    Guid Id,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate);