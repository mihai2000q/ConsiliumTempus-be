using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
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
        if (!Enum.TryParse<Permissions>(requirement.Permission, out var permission)) return;

        var subUserId = context.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (subUserId is null || !Guid.TryParse(subUserId, out var guidUserId)) return;

        using var scope = serviceScopeFactory.CreateScope();
        var permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var workspaceProvider = scope.ServiceProvider.GetRequiredService<IWorkspaceProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;

        var workspaceId = await GetWorkspaceId(request, workspaceProvider, permission);
        if (workspaceId is null)
        {
            context.Succeed(requirement); // let the system return the not found or validation error 
        }
        else
        {
            var userId = UserId.Create(guidUserId);
            var permissions = await permissionProvider.GetPermissions(userId, workspaceId);
            if (permissions.Contains(requirement.Permission)) context.Succeed(requirement);
        }
    }

    /// <summary>
    /// Get Workspace id from query, route or body of the request or related entity and
    /// return it or null when the workspace was either not found, or there is a validation error
    /// </summary>
    private static async Task<WorkspaceId?> GetWorkspaceId(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider,
        Permissions permission)
    {
        var stringId = await GetStringId(request, permission);

        if (string.IsNullOrWhiteSpace(stringId)) return null;
        if (!Guid.TryParse(stringId, out var guidId)) return null;

        var workspace = permission switch
        {
            Permissions.ReadProject or
            Permissions.ReadOverviewProject or
            Permissions.UpdateProject or
            Permissions.UpdateFavoritesProject or
            Permissions.UpdateOverviewProject or
            Permissions.DeleteProject or
            Permissions.CreateProjectSprint or
            Permissions.ReadCollectionProjectSprint or
            Permissions.AddStatusToProject or 
            Permissions.ReadStatusesFromProject or 
            Permissions.RemoveStatusFromProject or 
            Permissions.UpdateStatusFromProject => await workspaceProvider.GetByProject(ProjectId.Create(guidId)),

            Permissions.ReadProjectSprint or
            Permissions.UpdateProjectSprint or
            Permissions.DeleteProjectSprint or
            Permissions.AddStageToProjectSprint or
            Permissions.ReadStagesFromProjectSprint or
            Permissions.MoveStageFromProjectSprint or
            Permissions.UpdateStageFromProjectSprint or
            Permissions.RemoveStageFromProjectSprint => await workspaceProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),

            Permissions.CreateProjectTask or
            Permissions.ReadCollectionProjectTask => await workspaceProvider.GetByProjectStage(ProjectStageId.Create(guidId)),

            Permissions.ReadProjectTask or
            Permissions.MoveProjectTask or
            Permissions.UpdateProjectTask or
            Permissions.UpdateIsCompletedProjectTask or
            Permissions.UpdateOverviewProjectTask or
            Permissions.DeleteProjectTask => await workspaceProvider.GetByProjectTask(ProjectTaskId.Create(guidId)),

            _ => await workspaceProvider.Get(WorkspaceId.Create(guidId))
        };

        return workspace?.Id;
    }

    private static async Task<string?> GetStringId(HttpRequest request, Permissions permission)
    {
        return permission switch
        {
            // Workspace
            Permissions.ReadWorkspace => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadOverviewWorkspace => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadCollaboratorsFromWorkspace => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadInvitationsFromWorkspace => HttpRequestReader.GetStringIdFromQuery(request, ToIdProperty<WorkspaceAggregate>()),
            Permissions.InviteCollaboratorToWorkspace => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateWorkspace => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateFavoritesWorkspace => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateOverviewWorkspace => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteWorkspace => HttpRequestReader.GetStringIdFromRoute(request),

            // Project
            Permissions.CreateProject => await HttpRequestReader.GetStringIdFromBody(request, ToIdProperty<WorkspaceAggregate>()),
            Permissions.ReadProject => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadOverviewProject => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadCollectionProject => HttpRequestReader.GetStringIdFromQuery(request, ToIdProperty<WorkspaceAggregate>()),
            Permissions.UpdateProject => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateFavoritesProject => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateOverviewProject => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProject => HttpRequestReader.GetStringIdFromRoute(request),
            
            // Project - Project Status
            Permissions.AddStatusToProject => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.ReadStatusesFromProject => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.UpdateStatusFromProject => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.RemoveStatusFromProject => HttpRequestReader.GetStringIdFromRoute(request),

            // Project Sprint
            Permissions.CreateProjectSprint => await HttpRequestReader.GetStringIdFromBody(request, ToIdProperty<ProjectAggregate>()),
            Permissions.ReadProjectSprint => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadCollectionProjectSprint => HttpRequestReader.GetStringIdFromQuery(request, ToIdProperty<ProjectAggregate>()),
            Permissions.UpdateProjectSprint => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProjectSprint => HttpRequestReader.GetStringIdFromRoute(request),

            // Project Sprint - Project Stage
            Permissions.AddStageToProjectSprint => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.ReadStagesFromProjectSprint => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.MoveStageFromProjectSprint => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateStageFromProjectSprint => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.RemoveStageFromProjectSprint => HttpRequestReader.GetStringIdFromRoute(request),

            // Project Task
            Permissions.CreateProjectTask => await HttpRequestReader.GetStringIdFromBody(request, ToIdProperty<ProjectStage>()),
            Permissions.ReadProjectTask => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadCollectionProjectTask => HttpRequestReader.GetStringIdFromQuery(request, ToIdProperty<ProjectStage>()),
            Permissions.MoveProjectTask => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateProjectTask => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateIsCompletedProjectTask => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.UpdateOverviewProjectTask => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProjectTask => HttpRequestReader.GetStringIdFromRoute(request),

            _ => throw new ArgumentOutOfRangeException(nameof(permission))
        };
    }

    private static string ToIdProperty<T>()
    {
        var property = typeof(T).Name.TruncateAggregate();
        property = property[0].ToString().ToLower() + property[1..];
        return property.ToId();
    }
}