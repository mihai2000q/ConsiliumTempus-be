using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Get;

public sealed class GetProjectTaskRequest
{
    [FromRoute] public Guid Id { get; init; }
}