using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;

public sealed record GetCollectionProjectTaskRequest
{
    [FromQuery] public Guid ProjectStageId { get; init; }
}