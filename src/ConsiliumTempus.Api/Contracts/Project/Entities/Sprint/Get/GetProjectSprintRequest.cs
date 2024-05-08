using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Get;

public sealed class GetProjectSprintRequest
{
    [FromRoute] public Guid Id { get; set; }
}