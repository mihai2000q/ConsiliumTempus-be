using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Stage.GetCollection;

public sealed record GetCollectionProjectStageRequest
{ 
    [FromQuery] public Guid ProjectSprintId { get; set; }
}