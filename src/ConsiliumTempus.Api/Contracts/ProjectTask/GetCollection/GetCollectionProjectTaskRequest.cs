using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;

public sealed class GetCollectionProjectTaskRequest
{
    [FromQuery] public Guid ProjectStageId { get; set; }
}