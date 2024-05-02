using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectResponse(
    List<GetCollectionProjectResponse.ProjectResponse> Projects,
    int TotalCount,
    int? TotalPages)
{
    public sealed record ProjectResponse(
        Guid Id,
        string Name,
        string Description,
        bool IsFavorite,
        bool IsPrivate);
}