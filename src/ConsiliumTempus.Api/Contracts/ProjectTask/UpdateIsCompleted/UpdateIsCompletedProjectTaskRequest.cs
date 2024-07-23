namespace ConsiliumTempus.Api.Contracts.ProjectTask.UpdateIsCompleted;

public sealed record UpdateIsCompletedProjectTaskRequest(
    Guid Id,
    bool IsCompleted);