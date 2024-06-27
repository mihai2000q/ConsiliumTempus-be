using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.GetStages;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetStagesFromProjectSprintResponse(
    List<GetStagesFromProjectSprintResponse.ProjectStageResponse> Stages)
{
    public sealed record ProjectStageResponse(
        Guid Id,
        string Name);
}