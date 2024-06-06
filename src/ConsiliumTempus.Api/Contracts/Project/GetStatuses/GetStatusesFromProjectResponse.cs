using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Api.Contracts.Project.GetStatuses;

public sealed record GetStatusesFromProjectResponse(
    List<GetStatusesFromProjectResponse.ProjectStatusResponse> Statuses,
    int TotalCount)
{
    public sealed record ProjectStatusResponse(
        Guid Id,
        string Title,
        ProjectStatusType Status,
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