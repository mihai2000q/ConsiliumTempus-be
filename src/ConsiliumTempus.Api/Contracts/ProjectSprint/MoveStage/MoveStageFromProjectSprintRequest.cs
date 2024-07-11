namespace ConsiliumTempus.Api.Contracts.ProjectSprint.MoveStage;

public sealed record MoveStageFromProjectSprintRequest(
    Guid Id,
    Guid StageId,
    Guid OverStageId);