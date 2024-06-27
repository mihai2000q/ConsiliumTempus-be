using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.GetStages;

public sealed record GetStagesFromProjectSprintRequest
{
    [FromRoute] public Guid Id { get; init; }
}