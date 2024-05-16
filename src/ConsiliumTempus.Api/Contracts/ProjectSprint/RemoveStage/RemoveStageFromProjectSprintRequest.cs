using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.RemoveStage;

public sealed record RemoveStageFromProjectSprintRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromRoute] public Guid StageId { get; init; }
}