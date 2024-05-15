using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Get;

public sealed record GetProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
}