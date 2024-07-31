using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.GetAllowedMembers;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetAllowedMembersFromProjectResponse(
    List<GetAllowedMembersFromProjectResponse.UserResponse> AllowedMembers)
{
    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}