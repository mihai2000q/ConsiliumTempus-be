using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.GetAllowedMembers;

public sealed record GetAllowedMembersFromProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
}