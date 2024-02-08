using ConsiliumTempus.Domain.User.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Infrastructure.Authorization.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ConsiliumTempus.Infrastructure.Authorization.Permission;

public class PermissionAuthorizationHandler(
    IServiceScopeFactory serviceScopeFactory) 
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var jwtUserId = context.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        
        using var scope = serviceScopeFactory.CreateScope();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();
        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        
        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null) return;
        
        var userId = UserId.Create(jwtUserId);
        var workspaceId = WorkspaceId.Create(GetWorkspaceId(request));
        var permissions = await permissionService.GetPermissions(userId, workspaceId);

        if (permissions.Contains(requirement.Permission)) context.Succeed(requirement);
    }

    private static string GetWorkspaceId(HttpRequest request)
    {
        if (request.Method == "GET")
            return request.RouteValues["id"] as string ?? "";
        return "";
    }
}