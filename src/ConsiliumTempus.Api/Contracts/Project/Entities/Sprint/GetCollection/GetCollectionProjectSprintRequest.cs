using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;

public sealed record GetCollectionProjectSprintRequest
{
    [FromQuery] public Guid ProjectId { get; init; }
}