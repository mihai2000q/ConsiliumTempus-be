using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Get;

public sealed record GetProjectSprintRequest
{
    [FromRoute] public Guid Id { get; init; }
}