using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.LeavePrivate;

public sealed record LeavePrivateProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
};