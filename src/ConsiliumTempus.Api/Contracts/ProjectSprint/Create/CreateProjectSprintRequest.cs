namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Create;

public sealed record CreateProjectSprintRequest(
    Guid ProjectId,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate,
    bool KeepPreviousStages,
    CreateProjectSprintRequest.CreateProjectStatus? ProjectStatus)
{
    public sealed record CreateProjectStatus(
        string Title,
        string Status,
        string Description);
}