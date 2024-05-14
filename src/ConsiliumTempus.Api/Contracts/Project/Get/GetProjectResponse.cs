using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetProjectResponse(
    string Name,
    string Description,
    bool IsFavorite,
    bool IsPrivate,
    List<GetProjectResponse.ProjectSprintResponse> Sprints)
{
    public sealed record ProjectSprintResponse(
        Guid Id,
        string Name,
        DateOnly? StartDate,
        DateOnly? EndDate);
};