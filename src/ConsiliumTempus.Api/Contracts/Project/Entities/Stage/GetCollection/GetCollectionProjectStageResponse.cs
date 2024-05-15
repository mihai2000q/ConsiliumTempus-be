using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Stage.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectStageResponse(
    List<GetCollectionProjectStageResponse.ProjectStageResponse> Stages)
{
    public sealed record ProjectStageResponse(
        Guid Id,
        string Name);
}