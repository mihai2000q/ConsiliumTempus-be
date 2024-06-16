using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectResponse(
    List<GetCollectionProjectResponse.ProjectResponse> Projects,
    int TotalCount)
{
    public sealed record ProjectResponse(
        Guid Id,
        string Name,
        string Description,
        bool IsFavorite,
        string Lifecycle,
        UserResponse Owner,
        bool IsPrivate,
        ProjectStatusResponse? LatestStatus,
        DateTime CreatedDateTime);

    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);

    public sealed record ProjectStatusResponse(
        Guid Id,
        string Status,
        DateTime UpdatedDateTime);
}