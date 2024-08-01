using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.Security.Authorization.Http;
using ConsiliumTempus.Infrastructure.Security.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Permission;

public sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var subUserId = context.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(subUserId, out var guidUserId)) return;

        using var scope = serviceScopeFactory.CreateScope();
        var permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var workspaceProvider = scope.ServiceProvider.GetRequiredService<IWorkspaceProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;

        var workspaceId = await GetWorkspaceId(request, workspaceProvider, requirement.Permission);
        if (workspaceId is null)
        {
            context.Succeed(requirement); // let the system return the not found or validation error 
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            var permissions = await permissionProvider.GetPermissions(userId, workspaceId);
            if (permissions.Contains(requirement.Permission.ToString())) context.Succeed(requirement);
        }
    }

    /// <summary>
    /// Get Workspace id from query, route or body of the request or related entity
    /// <returns>
    /// the workspace or null when the workspace was either not found, or there is a validation error
    /// </returns>
    /// </summary>
    private static async Task<WorkspaceId?> GetWorkspaceId(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider,
        Permissions permission)
    {
        var (stringId, idType) = await GetStringId(request, permission);

        if (!Guid.TryParse(stringId, out var guidId)) return null;

        var workspace = idType switch
        {
            StringIdType.Workspace => await workspaceProvider.Get(WorkspaceId.Create(guidId)),
            StringIdType.Project => await workspaceProvider.GetByProject(ProjectId.Create(guidId)),
            StringIdType.ProjectSprint => await workspaceProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),
            StringIdType.ProjectStage => await workspaceProvider.GetByProjectStage(ProjectStageId.Create(guidId)),
            StringIdType.ProjectTask => await workspaceProvider.GetByProjectTask(ProjectTaskId.Create(guidId)),
            _ => throw new ArgumentOutOfRangeException(nameof(permission))
        };

        return workspace?.Id;
    }

    /// <summary>
    /// Instead of CRUD - it is "C.U.D.R.", because, most of the time,
    /// the id will be on body for a Create or Update Request,
    /// and on the route for a Delete or Read operation,
    /// and sometimes, as a query param for a reading operation
    /// </summary>
    private static async Task<(string?, StringIdType)> GetStringId(HttpRequest request, Permissions permission)
    {
        return permission switch
        {
            // Workspace
            Permissions.UpdateWorkspace or
            Permissions.UpdateFavoritesWorkspace or
            Permissions.UpdateOverviewWorkspace => (
                await HttpRequestReader.GetStringIdFromBody(request),
                StringIdType.Workspace),

            Permissions.DeleteWorkspace or
            Permissions.ReadWorkspace or
            Permissions.ReadOverviewWorkspace => (
                HttpRequestReader.GetStringIdFromRoute(request), 
                StringIdType.Workspace),

            Permissions.ReadInvitationsFromWorkspace => (
                HttpRequestReader.GetStringIdFromQuery(request, typeof(WorkspaceAggregate).ToCamelId()),
                StringIdType.Workspace),

            // Workspace - Collaborators
            Permissions.InviteCollaboratorToWorkspace or
            Permissions.UpdateCollaboratorFromWorkspace => (
                await HttpRequestReader.GetStringIdFromBody(request),
                StringIdType.Workspace),

            Permissions.KickCollaboratorFromWorkspace or
            Permissions.ReadCollaboratorsFromWorkspace => (
                HttpRequestReader.GetStringIdFromRoute(request),
                StringIdType.Workspace),

            // Project
            Permissions.CreateProject => (
                await HttpRequestReader.GetStringIdFromBody(request, typeof(WorkspaceAggregate).ToCamelId()),
                StringIdType.Workspace),

            Permissions.UpdateProject or
            Permissions.UpdateFavoritesProject or
            Permissions.UpdateOverviewProject => (
                await HttpRequestReader.GetStringIdFromBody(request),
                StringIdType.Project),

            Permissions.DeleteProject or
            Permissions.ReadProject or
            Permissions.ReadOverviewProject => (
                HttpRequestReader.GetStringIdFromRoute(request),
                StringIdType.Project),
            
            Permissions.ReadCollectionProject => (
                HttpRequestReader.GetStringIdFromQuery(request, typeof(WorkspaceAggregate).ToCamelId()),
                StringIdType.Workspace),

            // Project - Project Status
            Permissions.AddStatusToProject or
            Permissions.UpdateStatusFromProject => (
                await HttpRequestReader.GetStringIdFromBody(request),
                StringIdType.Project),

            Permissions.RemoveStatusFromProject or
            Permissions.ReadStatusesFromProject => (
                HttpRequestReader.GetStringIdFromRoute(request),
                StringIdType.Project),

            // Project - Allowed Members
            Permissions.ReadAllowedMembersFromProject => (
                HttpRequestReader.GetStringIdFromRoute(request),
                StringIdType.Project),

            // Project Sprint
            Permissions.CreateProjectSprint => (
                await HttpRequestReader.GetStringIdFromBody(request, typeof(ProjectAggregate).ToCamelId()),
                StringIdType.Project),

            Permissions.UpdateProjectSprint => (
                await HttpRequestReader.GetStringIdFromBody(request),
                StringIdType.ProjectSprint),

            Permissions.DeleteProjectSprint or
            Permissions.ReadProjectSprint => (
                HttpRequestReader.GetStringIdFromRoute(request),
                StringIdType.ProjectSprint),

            Permissions.ReadCollectionProjectSprint => (
                HttpRequestReader.GetStringIdFromQuery(request, typeof(ProjectAggregate).ToCamelId()),
                StringIdType.Project),

            // Project Sprint - Project Stage
            Permissions.AddStageToProjectSprint or
            Permissions.MoveStageFromProjectSprint or
            Permissions.UpdateStageFromProjectSprint => (
                await HttpRequestReader.GetStringIdFromBody(request),
                StringIdType.ProjectSprint),

            Permissions.RemoveStageFromProjectSprint or
            Permissions.ReadStagesFromProjectSprint => (
                HttpRequestReader.GetStringIdFromRoute(request),
                StringIdType.ProjectSprint),

            // Project Task
            Permissions.CreateProjectTask => (
                await HttpRequestReader.GetStringIdFromBody(request, typeof(ProjectStage).ToCamelId()),
                StringIdType.ProjectStage),

            Permissions.MoveProjectTask or
            Permissions.UpdateProjectTask or
            Permissions.UpdateIsCompletedProjectTask or
            Permissions.UpdateOverviewProjectTask => (
                await HttpRequestReader.GetStringIdFromBody(request),
                StringIdType.ProjectTask),

            Permissions.DeleteProjectTask or
            Permissions.ReadProjectTask => (
                HttpRequestReader.GetStringIdFromRoute(request),
                StringIdType.ProjectTask),

            Permissions.ReadCollectionProjectTask => (
                HttpRequestReader.GetStringIdFromQuery(request, typeof(ProjectStage).ToCamelId()),
                StringIdType.ProjectStage),

            _ => throw new ArgumentOutOfRangeException(nameof(permission))
        };
    }
    
    private enum StringIdType
    {
        Workspace,
        Project,
        ProjectSprint,
        ProjectStage,
        ProjectTask
    }
}