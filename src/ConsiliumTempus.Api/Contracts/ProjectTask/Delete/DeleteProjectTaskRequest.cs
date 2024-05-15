using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Delete;

public sealed record DeleteProjectTaskRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromQuery] public Guid StageId { get; init; }
}