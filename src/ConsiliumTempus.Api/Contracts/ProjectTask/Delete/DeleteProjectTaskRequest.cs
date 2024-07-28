using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Delete;

public sealed record DeleteProjectTaskRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromRoute] public Guid StageId { get; init; }
}