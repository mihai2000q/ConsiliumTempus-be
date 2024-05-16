using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetProjectSprintResponse(
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate,
    List<GetProjectSprintResponse.ProjectStageResponse> Stages)
{
    public sealed record ProjectStageResponse(
        Guid Id,
        string Name);
}