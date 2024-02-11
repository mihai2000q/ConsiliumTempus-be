using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Authorization.Permission;

public sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var jwtUserId = context.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

        using var scope = serviceScopeFactory.CreateScope();
        var permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var workspaceProvider = scope.ServiceProvider.GetRequiredService<IWorkspaceProvider>();

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;

        var userId = UserId.Create(jwtUserId);
        var res = await GetWorkspaceId(request, workspaceProvider);
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
        IWorkspaceProvider workspaceProvider)
    {
        var stringId = request.Method switch
        {
            "GET" => GetFromGetRequestWorkspaceStringId(request),
            "DELETE" => GetFromDeleteRequestWorkspaceStringId(request),
            _ => null
        };
        if (string.IsNullOrWhiteSpace(stringId)) return null;

        var workspaceId = WorkspaceId.Create(stringId);
        var workspace = await workspaceProvider.Get(workspaceId);

        return workspace is null
            ? new WorkspaceIdResponse(workspaceId, true)
            : new WorkspaceIdResponse(workspaceId, false);
    }

    private static string? GetFromGetRequestWorkspaceStringId(HttpRequest request)
    {
        if (request.RouteValues["controller"]?.Equals("Workspace") == true &&
            !string.IsNullOrEmpty((string?)request.RouteValues["id"]))
        {
            return (string?)request.RouteValues["id"];
        }

        return null;
    }
    
    private static string? GetFromDeleteRequestWorkspaceStringId(HttpRequest request)
    {
        if (request.RouteValues["controller"]?.Equals("Workspace") == true &&
            !string.IsNullOrEmpty((string?)request.RouteValues["id"]))
        {
            return (string?)request.RouteValues["id"];
        }

        return null;
    }

    private class WorkspaceIdResponse(WorkspaceId workspaceId, bool notFound)
    {
        internal WorkspaceId WorkspaceId { get; } = workspaceId;
        internal bool NotFound { get; } = notFound;
    }
}