using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectSprintResponse(
    List<GetCollectionProjectSprintResponse.ProjectSprintResponse> Sprints)
{
    public sealed record ProjectSprintResponse(
        Guid Id,
        string Name,
        DateOnly? StartDate,
        DateOnly? EndDate);
}