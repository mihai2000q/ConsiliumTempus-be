namespace ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Create;

public sealed record CreateProjectStageRequest(
    Guid ProjectSprintId,
    string Name);