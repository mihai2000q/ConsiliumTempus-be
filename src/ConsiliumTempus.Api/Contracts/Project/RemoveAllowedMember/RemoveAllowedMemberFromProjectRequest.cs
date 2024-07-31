using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.Contracts.Project.RemoveAllowedMember;

public sealed record RemoveAllowedMemberFromProjectRequest
{
    [FromRoute] public Guid Id { get; init; }
    [FromRoute] public Guid AllowedMemberId { get; init; }
}