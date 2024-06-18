using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectSprint.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectSprintResponse(
    List<GetCollectionProjectSprintResponse.ProjectSprintResponse> Sprints,
    int TotalCount)
{
    public sealed record ProjectSprintResponse(
        Guid Id,
        string Name,
        DateOnly? StartDate,
        DateOnly? EndDate);
}