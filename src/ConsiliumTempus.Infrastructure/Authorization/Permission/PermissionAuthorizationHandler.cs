using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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
        var stringId = request.RouteValues["controller"] switch
        {
            Controllers.Workspace => request.Method switch
            {
                HttpRequestType.GET => GetStringIdFromRoute(request),
                HttpRequestType.PUT => await GetStringIdFromBody(request),
                HttpRequestType.DELETE => GetStringIdFromRoute(request),
                _ => null
            },
            _ => null
        };

        if (string.IsNullOrWhiteSpace(stringId)) return null;

        var workspaceId = WorkspaceId.Create(stringId);
        var workspace = await workspaceProvider.Get(workspaceId);

        return workspace is null
            ? new WorkspaceIdResponse(workspaceId, true)
            : new WorkspaceIdResponse(workspaceId, false);
    }

    private static string? GetStringIdFromRoute(HttpRequest request)
    {
        var id = (string?)request.RouteValues["id"];

        return !string.IsNullOrEmpty(id) ? id : null;
    }

    private static async Task<string?> GetStringIdFromBody(HttpRequest request)
    {
        request.EnableBuffering();
        var body = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(request.Body);
        request.Body.Position = 0;
        
        var id = body?["id"] ?? "";
        return !string.IsNullOrEmpty(id) ? id : null;
    }

    private class WorkspaceIdResponse(WorkspaceId workspaceId, bool notFound)
    {
        internal WorkspaceId WorkspaceId { get; } = workspaceId;
        internal bool NotFound { get; } = notFound;
    }

    private static class Controllers
    {
        public const string Workspace = "Workspace";
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static class HttpRequestType
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
    }
}