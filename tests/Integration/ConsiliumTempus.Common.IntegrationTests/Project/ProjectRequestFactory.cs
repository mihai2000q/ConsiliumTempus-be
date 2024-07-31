using ConsiliumTempus.Api.Contracts.Project.AddAllowedMember;
using ConsiliumTempus.Api.Contracts.Project.AddStatus;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Delete;
using ConsiliumTempus.Api.Contracts.Project.Get;
using ConsiliumTempus.Api.Contracts.Project.GetAllowedMembers;
using ConsiliumTempus.Api.Contracts.Project.GetCollection;
using ConsiliumTempus.Api.Contracts.Project.GetOverview;
using ConsiliumTempus.Api.Contracts.Project.GetStatuses;
using ConsiliumTempus.Api.Contracts.Project.RemoveStatus;
using ConsiliumTempus.Api.Contracts.Project.Update;
using ConsiliumTempus.Api.Contracts.Project.UpdateFavorites;
using ConsiliumTempus.Api.Contracts.Project.UpdateOverview;
using ConsiliumTempus.Api.Contracts.Project.UpdatePrivacy;
using ConsiliumTempus.Api.Contracts.Project.UpdateStatus;
using ConsiliumTempus.Common.IntegrationTests.TestConstants;
using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Common.IntegrationTests.Project;

public static class ProjectRequestFactory
{
    public static GetProjectRequest CreateGetProjectRequest(Guid? id = null)
    {
        return new GetProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetOverviewProjectRequest CreateGetOverviewProjectRequest(Guid? id = null)
    {
        return new GetOverviewProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetCollectionProjectRequest CreateGetCollectionProjectRequest(
        int? pageSize = null,
        int? currentPage = null,
        string[]? orderBy = null,
        string[]? search = null,
        Guid? workspaceId = null)
    {
        return new GetCollectionProjectRequest
        {
            PageSize = pageSize,
            CurrentPage = currentPage,
            OrderBy = orderBy,
            Search = search,
            WorkspaceId = workspaceId
        };
    }

    public static GetStatusesFromProjectRequest CreateGetStatusesFromProjectRequest(Guid? id = null)
    {
        return new GetStatusesFromProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static GetAllowedMembersFromProjectRequest CreateGetAllowedMembersFromProjectRequest(Guid? id = null)
    {
        return new GetAllowedMembersFromProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static CreateProjectRequest CreateCreateProjectRequest(
        Guid? workspaceId = null,
        string name = Constants.Project.Name,
        bool isPrivate = false)
    {
        return new CreateProjectRequest(
            workspaceId ?? Guid.NewGuid(),
            name,
            isPrivate);
    }

    public static AddAllowedMemberToProjectRequest CreateAddAllowedMemberToProjectRequest(
        Guid? id = null,
        Guid? collaboratorId = null)
    {
        return new AddAllowedMemberToProjectRequest(
            id ?? Guid.NewGuid(),
            collaboratorId ?? Guid.NewGuid());
    }

    public static AddStatusToProjectRequest CreateAddStatusToProjectRequest(
        Guid? id = null,
        string title = Constants.ProjectStatus.Title,
        ProjectStatusType status = ProjectStatusType.AtRisk,
        string description = Constants.ProjectStatus.Description)
    {
        return new AddStatusToProjectRequest(
            id ?? Guid.NewGuid(),
            title,
            status.ToString(),
            description);
    }

    public static UpdateProjectRequest CreateUpdateProjectRequest(
        Guid? id = null,
        string name = Constants.Project.Name,
        ProjectLifecycle lifecycle = ProjectLifecycle.Active)
    {
        return new UpdateProjectRequest(
            id ?? Guid.NewGuid(),
            name,
            lifecycle.ToString());
    }

    public static UpdateFavoritesProjectRequest CreateUpdateFavoritesProjectRequest(
        Guid? id = null,
        bool isFavorite = false)
    {
        return new UpdateFavoritesProjectRequest(
            id ?? Guid.NewGuid(),
            isFavorite);
    }

    public static UpdatePrivacyProjectRequest CreateUpdatePrivacyProjectRequest(
        Guid? id = null,
        bool isPrivate = false)
    {
        return new UpdatePrivacyProjectRequest(
            id ?? Guid.NewGuid(),
            isPrivate);
    }

    public static UpdateOverviewProjectRequest CreateUpdateOverviewProjectRequest(
        Guid? id = null,
        string description = Constants.Project.Description)
    {
        return new UpdateOverviewProjectRequest(
            id ?? Guid.NewGuid(),
            description);
    }

    public static UpdateStatusFromProjectRequest CreateUpdateStatusFromProjectRequest(
        Guid? id = null,
        Guid? statusId = null,
        string title = Constants.ProjectStatus.Title,
        ProjectStatusType status = ProjectStatusType.AtRisk,
        string description = Constants.ProjectStatus.Description)
    {
        return new UpdateStatusFromProjectRequest(
            id ?? Guid.NewGuid(),
            statusId ?? Guid.NewGuid(),
            title,
            status.ToString(),
            description);
    }

    public static DeleteProjectRequest CreateDeleteProjectRequest(Guid? id = null)
    {
        return new DeleteProjectRequest
        {
            Id = id ?? Guid.NewGuid()
        };
    }

    public static RemoveStatusFromProjectRequest CreateRemoveStatusFromProjectRequest(
        Guid? id = null,
        Guid? statusId = null)
    {
        return new RemoveStatusFromProjectRequest
        {
            Id = id ?? Guid.NewGuid(),
            StatusId = statusId ?? Guid.NewGuid(),
        };
    }
}