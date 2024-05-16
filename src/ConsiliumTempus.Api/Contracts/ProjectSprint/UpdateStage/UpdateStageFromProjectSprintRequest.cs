namespace ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;

public sealed record UpdateStageFromProjectSprintRequest(
    Guid Id,
    Guid StageId,
    string Name);