using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.UpdateStage;

public sealed record UpdateStageFromProjectSprintRequest(
    string Name)
{
    [FromRoute] public Guid Id { get; init; }
    [FromRoute] public Guid StageId { get; init; }
}