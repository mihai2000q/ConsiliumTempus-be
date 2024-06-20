namespace ConsiliumTempus.Api.Contracts.ProjectTask.UpdateOverview;

public sealed record UpdateOverviewProjectTaskRequest(
    Guid Id,
    string Name,
    string Description,
    bool IsCompleted,
    Guid? AssigneeId);