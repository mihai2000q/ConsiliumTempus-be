using ConsiliumTempus.Application.Project.Commands.AddStatus;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Commands.RemoveStatus;
using ConsiliumTempus.Application.Project.Commands.Update;
using ConsiliumTempus.Application.Project.Commands.UpdateOverview;
using ConsiliumTempus.Application.Project.Commands.UpdateStatus;
using ConsiliumTempus.Common.UnitTests.TestConstants;
using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Common.UnitTests.Project;

public static class ProjectCommandFactory
{
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
        bool isFavorite = false)
    {
        return new UpdateProjectCommand(
            id ?? Guid.NewGuid(),
            name,
            isFavorite);
    }
    
    public static UpdateOverviewProjectCommand CreateUpdateOverviewProjectCommand(
        Guid? id = null,
        string name = Constants.Project.Description)
    {
        return new UpdateOverviewProjectCommand(
            id ?? Guid.NewGuid(),
            name);
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