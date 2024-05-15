using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;

public sealed record GetProjectSprintRequest
{
    [FromRoute] public Guid Id { get; init; }
}