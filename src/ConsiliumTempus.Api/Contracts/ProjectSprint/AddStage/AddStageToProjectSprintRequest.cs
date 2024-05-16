using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.AddStage;

public sealed record AddStageToProjectSprintRequest(
    string Name,
    bool OnTop)
{
    [FromRoute] public Guid Id { get; set; }
}