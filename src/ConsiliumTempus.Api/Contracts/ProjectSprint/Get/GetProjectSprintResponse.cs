using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetProjectSprintResponse(
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate,
    List<GetProjectSprintResponse.ProjectStageResponse> Stages,
    GetProjectSprintResponse.UserResponse? CreatedBy,
    DateTime CreatedDateTime,
    GetProjectSprintResponse.UserResponse? UpdatedBy,
    DateTime UpdatedDateTime)
{
    public sealed record ProjectStageResponse(
        Guid Id,
        string Name);

    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}