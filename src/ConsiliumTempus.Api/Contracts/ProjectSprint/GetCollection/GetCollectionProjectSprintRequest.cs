using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;

public sealed record GetCollectionProjectSprintRequest
{
    [FromQuery] public Guid ProjectId { get; init; }
}