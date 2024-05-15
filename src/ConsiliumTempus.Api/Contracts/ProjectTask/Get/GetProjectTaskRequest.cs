using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Get;

public sealed record GetProjectTaskRequest
{
    [FromRoute] public Guid Id { get; init; }
}