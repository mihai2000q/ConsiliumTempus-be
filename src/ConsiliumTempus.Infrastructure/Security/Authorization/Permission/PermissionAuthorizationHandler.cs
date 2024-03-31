using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User.ValueObjects;
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
        var subUserId = context.User.Claims
            .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (subUserId is null || !Guid.TryParse(subUserId, out var guidUserId)) return;

        using var scope = serviceScopeFactory.CreateScope();
        var permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var workspaceProvider = scope.ServiceProvider.GetRequiredService<IWorkspaceProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;
        
        var userId = UserId.Create(guidUserId);
        var res = await GetWorkspaceId(request, workspaceProvider, requirement.Permission);
        if (res is null) return;
        if (res.NotFound)
        {
            context.Succeed(requirement); // let the system return the not found error
        }
        else
        {
            var permissions = await permissionProvider.GetPermissions(userId, res.WorkspaceId);
            if (permissions.Contains(requirement.Permission)) context.Succeed(requirement);
        }
    }

    private static async Task<WorkspaceIdResponse?> GetWorkspaceId(
        HttpRequest request,
        IWorkspaceProvider workspaceProvider,
        string permission)
    {
        if (!Enum.TryParse<Permissions>(permission, out var enumPermission)) return null;
        var stringId = await GetStringId(request, enumPermission);

        if (string.IsNullOrWhiteSpace(stringId)) return null;
        if (!Guid.TryParse(stringId, out var guidId)) return null;

        var workspace = enumPermission switch
        {
            Permissions.ReadProject or
            Permissions.UpdateProject or
            Permissions.DeleteProject or
            Permissions.CreateProjectSprint => await workspaceProvider.GetByProject(ProjectId.Create(guidId)),
            
            Permissions.UpdateProjectSprint or
            Permissions.DeleteProjectSprint => await workspaceProvider.GetByProjectSprint(ProjectSprintId.Create(guidId)),
            
            _ => await workspaceProvider.Get(WorkspaceId.Create(guidId)),
        };

        return workspace is null
            ? new WorkspaceIdResponse(default!, true)
            : new WorkspaceIdResponse(workspace.Id, false);
    }

    private static async Task<string?> GetStringId(HttpRequest request, Permissions permission)
    {
        return permission switch
        {
            Permissions.ReadWorkspace => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.UpdateWorkspace => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteWorkspace => HttpRequestReader.GetStringIdFromRoute(request),
            
            Permissions.CreateProject => await HttpRequestReader.GetPropertyFromBody(request, "workspaceId"),
            Permissions.ReadProject => HttpRequestReader.GetStringIdFromRoute(request),
            Permissions.UpdateProject => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProject => HttpRequestReader.GetStringIdFromRoute(request),
            
            Permissions.CreateProjectSprint => await HttpRequestReader.GetPropertyFromBody(request, "projectId"),
            Permissions.UpdateProjectSprint => await HttpRequestReader.GetStringIdFromBody(request),
            Permissions.DeleteProjectSprint => HttpRequestReader.GetStringIdFromRoute(request),
            _ => throw new ArgumentOutOfRangeException(nameof(permission))
        };
    }

    private record WorkspaceIdResponse(WorkspaceId WorkspaceId, bool NotFound);
}