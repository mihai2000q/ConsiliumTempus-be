using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Delete;

public sealed record DeleteProjectSprintRequest
{
    [FromRoute] public Guid Id { get; init; }
};