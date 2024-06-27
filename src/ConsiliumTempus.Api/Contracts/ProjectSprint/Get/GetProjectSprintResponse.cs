using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetProjectSprintResponse(
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate,
    GetProjectSprintResponse.UserResponse? CreatedBy,
    DateTime CreatedDateTime,
    GetProjectSprintResponse.UserResponse? UpdatedBy,
    DateTime UpdatedDateTime)
{
    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}