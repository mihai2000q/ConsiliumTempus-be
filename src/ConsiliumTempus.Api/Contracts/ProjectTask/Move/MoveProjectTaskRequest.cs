namespace ConsiliumTempus.Api.Contracts.ProjectTask.Move;

public sealed record MoveProjectTaskRequest(
    Guid SprintId,
    Guid Id,
    Guid OverId);