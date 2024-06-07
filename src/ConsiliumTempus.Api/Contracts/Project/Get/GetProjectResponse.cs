using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetProjectResponse(
    string Name,
    bool IsFavorite,
    string Lifecycle,
    GetProjectResponse.UserResponse Owner,
    bool IsPrivate,
    GetProjectResponse.ProjectStatusResponse? LatestStatus)
{
    public sealed record ProjectStatusResponse(
        Guid Id,
        string Title,
        string Status,
        UserResponse? CreatedBy,
        DateTime CreatedDateTime,
        UserResponse? UpdatedBy,
        DateTime UpdatedDateTime);

    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}