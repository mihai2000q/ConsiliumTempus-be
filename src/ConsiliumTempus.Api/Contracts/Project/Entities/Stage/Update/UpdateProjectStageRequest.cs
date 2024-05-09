namespace ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Update;

public sealed record UpdateProjectStageRequest(
    Guid Id,
    string Name);