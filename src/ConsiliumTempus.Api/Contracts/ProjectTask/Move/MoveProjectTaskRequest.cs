namespace ConsiliumTempus.Api.Contracts.ProjectTask.Move;

public sealed record MoveProjectTaskRequest(
    Guid Id,
    Guid OverId);