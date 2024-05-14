using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Delete;

public sealed class DeleteProjectTaskRequest
{
    [FromRoute] public Guid Id { get; set; }
    [FromQuery] public Guid StageId { get; set; }
}