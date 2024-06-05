using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.RemoveStatus;

public sealed record RemoveStatusFromProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromRoute] public Guid StatusId { get; init; }
}