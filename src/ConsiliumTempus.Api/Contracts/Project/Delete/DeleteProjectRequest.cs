using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Delete;

public sealed record DeleteProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
};