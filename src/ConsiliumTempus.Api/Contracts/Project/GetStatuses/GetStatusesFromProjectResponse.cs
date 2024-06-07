using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.Project.GetStatuses;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetStatusesFromProjectResponse(
    List<GetStatusesFromProjectResponse.ProjectStatusResponse> Statuses,
    int TotalCount)
{
    public sealed record ProjectStatusResponse(
        Guid Id,
        string Title,
        string Status,
        string Description,
        UserResponse? CreatedBy,
        DateTime CreatedDateTime,
        UserResponse? UpdatedBy,
        DateTime UpdatedDateTime);
    
    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);
}