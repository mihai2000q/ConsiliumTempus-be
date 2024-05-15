using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Stage.Delete;

public sealed record DeleteProjectStageRequest
{
    [FromRoute] public Guid Id { get; init; }
}