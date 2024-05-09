using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
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
            Permissions.UpdateProject or
            Permissions.DeleteProject or 
            Permissions.CreateProjectSprint or
            Permissions.ReadCollectionProjectSprint => await workspaceProvider.GetByProject(ProjectId.Create(guidId)),

            Permissions.ReadProjectSprint or
            Permissions.UpdateProjectSprint or
            Permissions.DeleteProjectSprint or
            Permissions.CreateProjectStage => await workspaceProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),
            
            Permissions.UpdateProjectStage or
            Permissions.DeleteProjectStage => await workspaceProvider.GetByProjectStage(ProjectStageId.Create(guidId)),

            _ => await workspaceProvider.Get(WorkspaceId.Create(guidId)),
        };

        return workspace?.Id;
    }

    private static async Task<string?> GetStringId(HttpRequest request, Permissions permission)
    {
        return permission switch
        {
            Permissions.ReadWorkspace => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.UpdateWorkspace => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteWorkspace => HttpRequestReader.GetStringIdFromRoute(request),

            Permissions.CreateProject => await HttpRequestReader.GetStringIdFromBody(request, ToIdProperty<WorkspaceAggregate>()),
            Permissions.ReadProject => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadCollectionProject => HttpRequestReader.GetStringIdFromQuery(request, ToIdProperty<WorkspaceAggregate>()),
            Permissions.UpdateProject => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProject => HttpRequestReader.GetStringIdFromRoute(request),

            Permissions.CreateProjectSprint => await HttpRequestReader.GetStringIdFromBody(request, ToIdProperty<ProjectAggregate>()),
            Permissions.ReadProjectSprint => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.ReadCollectionProjectSprint => HttpRequestReader.GetStringIdFromQuery(request, ToIdProperty<ProjectAggregate>()),
            Permissions.UpdateProjectSprint => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProjectSprint => HttpRequestReader.GetStringIdFromRoute(request),
            
            Permissions.CreateProjectStage => await HttpRequestReader.GetStringIdFromBody(request, ToIdProperty<ProjectSprint>()),
            Permissions.UpdateProjectStage => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProjectStage => HttpRequestReader.GetStringIdFromRoute(request),
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