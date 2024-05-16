namespace ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;

public sealed record AddStageToProjectSprintRequest(
    Guid Id,
    string Name,
    bool OnTop);