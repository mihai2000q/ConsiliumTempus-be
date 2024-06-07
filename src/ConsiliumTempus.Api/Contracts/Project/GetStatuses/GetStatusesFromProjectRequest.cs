using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetStatuses;

public sealed record GetStatusesFromProjectRequest
{
    [FromRoute] public Guid Id { get; init; }  
}