using ConsiliumTempus.Application.Project.Commands.AddAllowedMember;
using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.RemoveAllowedMember;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateFavorites;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateOwner;
using ConsiliumTempus.Application.Project.Commands.UpdatePrivacy;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectCommandFactory
{
    public static AddAllowedMemberToProjectCommand CreateAddAllowedMemberToProjectCommand(
        Guid? id = null,
        Guid? collaboratorId = null)
    {
        return new AddAllowedMemberToProjectCommand(
            id ?? Guid.NewGuid(),
            collaboratorId ?? Guid.NewGuid());
    }

    public static AddStatusToProjectCommand CreateAddStatusToProjectCommand(
        Guid? id = null,
        string title = Constants.ProjectStatus.Title,
        ProjectStatusType status = ProjectStatusType.OnTrack,
        string description = Constants.ProjectStatus.Description)
    {
        return new AddStatusToProjectCommand(
            id ?? Guid.NewGuid(),
            title,
            status.ToString(),
            description);
    }
    
    public static AddStatusToProjectCommand CreateAddStatusToProjectCommandWithStatus(string status)
    {
        return new AddStatusToProjectCommand(
            Guid.NewGuid(),
            Constants.ProjectStatus.Title,
            status,
            Constants.ProjectStatus.Description);
    }
    
    public static CreateProjectCommand CreateCreateProjectCommand(
        Guid? workspaceId = null,
        string name = Constants.Project.Name,
        bool isPrivate = false)
    {
        return new CreateProjectCommand(
            workspaceId ?? Guid.NewGuid(),
            name,
            isPrivate);
    }
    
    public static DeleteProjectCommand CreateDeleteProjectCommand(Guid? id = null)
    {
        return new DeleteProjectCommand(id ?? Guid.NewGuid());
    }

    public static RemoveAllowedMemberFromProjectCommand CreateRemoveAllowedMemberFromProjectCommand(
        Guid? id = null,
        Guid? allowedMemberId = null)
    {
        return new RemoveAllowedMemberFromProjectCommand(
            id ?? Guid.NewGuid(),
            allowedMemberId ?? Guid.NewGuid());
    }

    public static RemoveStatusFromProjectCommand CreateRemoveStatusFromProjectCommand(
        Guid? id = null,
        Guid? statusId = null)
    {
        return new RemoveStatusFromProjectCommand(
            id ?? Guid.NewGuid(),
            statusId ?? Guid.NewGuid());
    }
    
    public static UpdateProjectCommand CreateUpdateProjectCommand(
        Guid? id = null,
        string name = Constants.Project.Name,
        ProjectLifecycle lifecycle = ProjectLifecycle.Active)
    {
        return new UpdateProjectCommand(
            id ?? Guid.NewGuid(),
            name,
            lifecycle.ToString());
    }
    
    public static UpdateProjectCommand CreateUpdateProjectCommandWithLifecycle(string lifecycle)
    {
        return new UpdateProjectCommand(
            Guid.NewGuid(),
            Constants.Project.Name,
            lifecycle);
    }

    public static UpdateFavoritesProjectCommand CreateUpdateFavoritesProjectCommand(
        Guid? id = null,
        bool isFavorite = false)
    {
        return new UpdateFavoritesProjectCommand(
            id ?? Guid.NewGuid(),
            isFavorite);
    }

    public static UpdatePrivacyProjectCommand CreateUpdatePrivacyProjectCommand(
        Guid? id = null,
        bool isPrivate = false)
    {
        return new UpdatePrivacyProjectCommand(
            id ?? Guid.NewGuid(),
            isPrivate);
    }
    
    public static UpdateOverviewProjectCommand CreateUpdateOverviewProjectCommand(
        Guid? id = null,
        string name = Constants.Project.Description)
    {
        return new UpdateOverviewProjectCommand(
            id ?? Guid.NewGuid(),
            name);
    }

    public static UpdateOwnerProjectCommand CreateUpdateOwnerProjectCommand(
        Guid? id = null,
        Guid? ownerId = null)
    {
        return new UpdateOwnerProjectCommand(
            id ?? Guid.NewGuid(),
            ownerId ?? Guid.NewGuid());
    }
    
    public static UpdateStatusFromProjectCommand CreateUpdateStatusFromProjectCommand(
        Guid? id = null,
        Guid? statusId = null,
        string title = Constants.ProjectStatus.Title,
        ProjectStatusType status = ProjectStatusType.OnTrack,
        string description = Constants.ProjectStatus.Description)
    {
        return new UpdateStatusFromProjectCommand(
            id ?? Guid.NewGuid(),
            statusId ?? Guid.NewGuid(),
            title,
            status.ToString(),
            description);
    }
    
    public static UpdateStatusFromProjectCommand CreateUpdateStatusFromProjectCommandWithStatus(string status)
    {
        return new UpdateStatusFromProjectCommand(
            Guid.NewGuid(), 
            Guid.NewGuid(),
            Constants.ProjectStatus.Title,
            status,
            Constants.ProjectStatus.Description);
    }
}