namespace ConsiliumTempus.Api.Contracts.ProjectTask.Create;

public sealed record CreateProjectTaskRequest(
    Guid ProjectStageId,
    string Name);