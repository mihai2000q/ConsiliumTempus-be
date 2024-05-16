using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Delete;

public sealed record DeleteProjectSprintRequest
{
    [FromRoute] public Guid Id { get; init; }
};